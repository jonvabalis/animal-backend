using animal_backend_api.Security;
using animal_backend_core.Commands;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos.Users;
using animal_backend_domain.Dtos.Animals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace animal_backend_api.Controllers;

public class UsersController(IWebHostEnvironment environment) : BaseController
{
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Allowed: jpg, jpeg, png, gif, webp");

        // Validate file size (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size exceeds 5MB limit");

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsFolder = Path.Combine(environment.ContentRootPath, "wwwroot", "uploads", "profiles");
        
        // Ensure directory exists
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the URL path that frontend can use
        var photoUrl = $"/uploads/profiles/{fileName}";
        
        return Ok(new { photoUrl });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserMeDto>> GetMe(CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var dto = await Mediator.Send(new GetMyProfileQuery(userId), ct);
        return Ok(dto);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(
        [FromBody] UpdateMyProfileDto dto,
        CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);

        await Mediator.Send(new UpdateMyProfileCommand(
            userId,
            dto.Name,
            dto.Surname,
            dto.PhoneNumber,
            dto.PhotoUrl
        ), ct);

        return NoContent();
    }

    [Authorize]
    [HttpPut("me/password")]
    public async Task<IActionResult> ChangeMyPassword(
        [FromBody] ChangeMyPasswordDto dto,
        CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);

        await Mediator.Send(new ChangeMyPasswordCommand(
            userId,
            dto.CurrentPassword,
            dto.NewPassword
        ), ct);

        return NoContent();
    }

    [Authorize]
    [HttpGet("me/animals")]
    public async Task<ActionResult<List<AnimalDto>>> GetMyAnimals(CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var animals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        return Ok(animals);
    }

    [Authorize]
    [HttpPost("me/animals")]
    public async Task<ActionResult<Guid>> CreateMyAnimal(
        [FromBody] CreateMyAnimalDto dto,
        CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);

        var dateOfBirthUtc = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc);

        var id = await Mediator.Send(new CreateMyAnimalCommand(
            userId,
            dto.Name,
            dto.Class,
            dto.PhotoUrl,
            dto.Breed,
            dto.Species,
            dto.SpeciesLatin,
            dateOfBirthUtc,
            dto.Weight
        ), ct);

        return Ok(id);
    }

    [Authorize]
    [HttpPut("me/animals/{id:guid}")]
    public async Task<IActionResult> UpdateMyAnimal(
        Guid id,
        [FromBody] UpdateMyAnimalDto dto,
        CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);

        var dateOfBirthUtc = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc);

        await Mediator.Send(new UpdateMyAnimalCommand(
            userId,
            id,
            dto.Name,
            dto.Class,
            dto.PhotoUrl,
            dto.Breed,
            dto.Species,
            dto.SpeciesLatin,
            dateOfBirthUtc,
            dto.Weight
        ), ct);

        return NoContent();
    }

    [HttpDelete("me/animals/{id:guid}")]
    public async Task<IActionResult> DeleteMyAnimal(Guid id, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);

        await Mediator.Send(new DeleteMyAnimalCommand(userId, id), ct);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe(CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        await Mediator.Send(new DeleteMyProfileCommand(userId), ct);
        return NoContent();
    }
}