namespace animal_backend_domain.Dtos.Users;

public class UpdateMyProfileDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
}