namespace animal_backend_domain.Dtos.Users;

public class ChangeMyPasswordDto
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}