using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;

using animal_backend_domain.Dtos;
using animal_backend_domain.Entities;

namespace animal_backend_api.Controllers;

public class AnimalController(IWebHostEnvironment environment) : BaseController
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
        var uploadsFolder = Path.Combine(environment.ContentRootPath, "wwwroot", "uploads", "animals");
        
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
        var photoUrl = $"/uploads/animals/{fileName}";
        
        return Ok(new { photoUrl });
    }

    // Additional endpoints related to animals (Illnesses, ProductsUsed, vaccines)
    
    [HttpGet("{animalId:guid}/vaccines")]
    public async Task<IActionResult> GetAllVaccines([FromRoute] Guid animalId)
    {
        return Ok(await Mediator.Send(new GetAllVaccinesQuery(animalId)));
    }

    [HttpGet("{animalId:guid}/vaccines/{id:guid}")]
    public async Task<IActionResult> GetByIdVaccine([FromRoute] Guid animalId, [FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVaccineQuery(animalId, id)));
    }

    [HttpPost("{animalId:guid}/vaccines")]
    public async Task<IActionResult> CreateVaccine([FromRoute] Guid animalId, [FromBody] Models.CreateVaccineRequest dto)
    {
        // Accept date as a flexible string from clients and parse to UTC.
        DateTime dateUtc;
        if (!string.IsNullOrWhiteSpace(dto?.Date) && DateTime.TryParse(dto.Date, out var parsed))
        {
            dateUtc = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
        }
        else
        {
            dateUtc = DateTime.UtcNow;
        }

        var command = new CreateVaccineCommand(
            dto?.Name ?? string.Empty,
            dateUtc,
            dto?.Description ?? string.Empty,
            dto?.Manufacturer ?? string.Empty,
            animalId
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{animalId:guid}/vaccines/{id:guid}")]
    public async Task<IActionResult> UpdateVaccine(Guid animalId, Guid id, [FromBody] Models.UpdateVaccineRequest dto)
    {
        DateTime dateUtc;
        if (!string.IsNullOrWhiteSpace(dto?.Date) && DateTime.TryParse(dto.Date, out var parsed))
        {
            dateUtc = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
        }
        else
        {
            dateUtc = DateTime.UtcNow;
        }

        var updateCommand = new UpdateVaccineCommand(
            id,
            dto?.Name ?? string.Empty,
            dateUtc,
            dto?.Description ?? string.Empty,
            dto?.Manufacturer ?? string.Empty,
            animalId
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{animalId:guid}/vaccines/{id:guid}")]
    public async Task<IActionResult> DeleteVaccine(Guid animalId, Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVaccineCommand(animalId, id)));
    }
    

    ////////////////////////////////////////////////////////////
    /// Sirgimas - Illnesses
    /// //////////////////////////////////////////////////////////
    
     [HttpGet("{animalId:guid}/illnesses")]
    public async Task<IActionResult> GetAllIllnesses([FromRoute] Guid animalId)
    {
        return Ok(await Mediator.Send(new GetAllIllnessesQuery(animalId)));
    }

    [HttpGet("{animalId:guid}/illnesses/{id:guid}")]
    public async Task<IActionResult> GetByIdIllness([FromRoute] Guid animalId, [FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdIllnessQuery(animalId, id)));
    }

    [HttpPost("{animalId:guid}/illnesses")]
    public async Task<IActionResult> CreateIllness([FromRoute] Guid animalId, [FromBody] IllnessInfoDto dto)
    {
        var command = new CreateIllnessCommand(
            dto.Name,
            dto.Description,        
            dto.DateDiagnosed,
            animalId,
            dto.DiseaseId
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{animalId:guid}/illnesses/{id:guid}")]
    public async Task<IActionResult> UpdateIllness(Guid animalId, Guid id, [FromBody] UpdateIllnessCommand command)
    {
        var updateCommand = new UpdateIllnessCommand(
            id,
            command.Name,
            command.Description,        
            command.DateDiagnosed,
            animalId,
            command.DiseaseId
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{animalId:guid}/illnesses/{id:guid}")]
    public async Task<IActionResult> DeleteIllness(Guid animalId, Guid id)
    {
        return Ok(await Mediator.Send(new DeleteIllnessCommand(animalId, id)));
    }


    ////////////////////////////////////////////////////////////
    /// Naudojami produktai - ProductsUsed
    /// //////////////////////////////////////////////////////////
    /// 
    
    [HttpGet("{animalId:guid}/productused")]
    public async Task<IActionResult> GetAllProductUsed([FromRoute] Guid animalId)
    {
        return Ok(await Mediator.Send(new GetAllProductsUsedQuery(animalId)));
    }

    [HttpGet("{animalId:guid}/productused/{id:guid}")]
    public async Task<IActionResult> GetByIdProductUsed([FromRoute] Guid animalId, [FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdProductUsedQuery(animalId, id)));
    }
    [HttpPost("{animalId:guid}/productused")]
    public async Task<IActionResult> CreateProductUsed([FromRoute] Guid animalId, [FromBody] ProductUsedInfoDto dto)
    {
        var command = new CreateProductUsedCommand(
            dto.Dosage,
            dto.TimesPerDay,
            animalId,
            dto.ProductId
        );  
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{animalId:guid}/productused/{id:guid}")]
    public async Task<IActionResult> UpdateProductUsed(Guid animalId, Guid id, [FromBody] UpdateProductUsedCommand command)
    {
        var updateCommand = new UpdateProductUsedCommand(
            id,
            command.Dosage,
            command.TimesPerDay,
            animalId,
            command.ProductId
        );
        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{animalId:guid}/productused/{id:guid}")]
    public async Task<IActionResult> DeleteProductUsed(Guid animalId, Guid id)
    {
        return Ok(await Mediator.Send(new DeleteProductUsedCommand(animalId, id)));
    }
}