using animal_backend_core.Commands;
using animal_backend_infrastructure;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using animal_backend_api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<animal_backend_core.Security.JwtTokenService>();

// Add services to the container.
builder.Services.AddSingleton(_ =>
{
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        ?? builder.Configuration["OpenAI:ApiKey"];

    if (string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("OpenAI API key is not configured.");
    }
    return new OpenAIClient(apiKey);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddControllers();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddDbContext<AnimalDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Authentication/Authorization
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
var jwtIssuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
var jwtAudience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//updated swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "animal-backend-api", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {your JWT token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddScoped<animal_backend_core.Security.JwtTokenService>();

// CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Ensure uploads directory exists
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads", "products");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Serve static files (for uploaded images) - must be before UseHttpsRedirection
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR hub
app.MapHub<ChatHub>("/chathub");

app.Run();