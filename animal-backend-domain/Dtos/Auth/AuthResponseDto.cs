namespace animal_backend_domain.Dtos.Auth;

public class AuthResponseDto
{
    public required string AccessToken { get; set; }
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}