using System.Runtime.CompilerServices;
using animal_backend_core.Commands;
using animal_backend_infrastructure;
using Microsoft.EntityFrameworkCore;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(sp =>
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
builder.Services.AddDbContext<AnimalDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();