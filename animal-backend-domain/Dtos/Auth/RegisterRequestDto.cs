namespace animal_backend_domain.Dtos.Auth;

public class RegisterRequestDto
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public string PhotoUrl { get; set; } = "";
}