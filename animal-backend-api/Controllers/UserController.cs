using animal_backend_domain.Dtos;
using animal_backend_api.Security;
using animal_backend_core.Commands;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos.Users;
using animal_backend_domain.Dtos.Animals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace animal_backend_api.Controllers;

public class UsersController : BaseController
{
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

        var id = await Mediator.Send(new CreateMyAnimalCommand(
            userId,
            dto.Name,
            dto.Class,
            dto.PhotoUrl,
            dto.Breed,
            dto.Species,
            dto.SpeciesLatin,
            dto.DateOfBirth,
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

        await Mediator.Send(new UpdateMyAnimalCommand(
            userId,
            id,
            dto.Name,
            dto.Class,
            dto.PhotoUrl,
            dto.Breed,
            dto.Species,
            dto.SpeciesLatin,
            dto.DateOfBirth,
            dto.Weight
        ), ct);

        return NoContent();
    }

    [Authorize]
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

    //////////////////////////////////////////////////////////////
    ////////////////////////////ILLNESSES//////////////////////////
    //////////////////////////////////////////////////////////////
    [Authorize]
    [HttpGet("me/animals/{animalId:guid}/illnesses")]
    public async Task<IActionResult> GetAllIllnesses([FromRoute] Guid animalId, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        return Ok(await Mediator.Send(new GetAllIllnessesQuery(animalId), ct));
    }

    [Authorize]
    [HttpGet("me/animals/illnesses/{id:guid}")]
    public async Task<IActionResult> GetIllnessById([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var illness = await Mediator.Send(new GetByIdIllnessQuery(id), ct);
        if (illness is null)
            return NotFound();

        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == illness.AnimalId))
            return Forbid();

        return Ok(illness);
    }

    [Authorize]
    [HttpPost("me/animals/{animalId:guid}/illnesses")]
    public async Task<IActionResult> CreateIllness([FromRoute] Guid animalId, [FromBody] IllnessInfoDto dto, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        var command = new CreateIllnessCommand(
            animalId,
            dto.Name,
            dto.Description,
            dto.DateDiagnosed,
            dto.DiseaseId
        );

        var id = await Mediator.Send(command, ct);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("me/animals/illnesses/{id:guid}")]
    public async Task<IActionResult> UpdateIllness([FromRoute] Guid id, [FromBody] UpdateIllnessCommand command, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var illness = await Mediator.Send(new GetByIdIllnessQuery(id), ct);
        if (illness is null)
            return NotFound();

        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == illness.AnimalId))
            return Forbid();

        var updateCommand = new UpdateIllnessCommand(
            id,
            command.Name,
            command.Description,
            command.DateDiagnosed
        );

        await Mediator.Send(updateCommand, ct);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me/animals/illnesses/{id:guid}")]
    public async Task<IActionResult> DeleteIllness([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var illness = await Mediator.Send(new GetByIdIllnessQuery(id), ct);
        if (illness is null)
            return NotFound();

        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == illness.AnimalId))
            return Forbid();

        await Mediator.Send(new DeleteIllnessCommand(id), ct);
        return NoContent();
    }

    //////////////////////////////////////////////////////////////
    ////////////////////////////ILLNESSES//////////////////////////
    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    ////////////////////////////VACCINES//////////////////////////
    //////////////////////////////////////////////////////////////
    [Authorize]
    [HttpGet("me/animals/vaccines")]
    public async Task<IActionResult> GetAllVaccines([FromQuery] GetAllVaccinesQuery query)
    {
        // allow optional filtering by query.AnimalId
        return Ok(await Mediator.Send(query));
    }

    [Authorize]
    [HttpGet("me/animals/{animalId:guid}/vaccines")]
    public async Task<IActionResult> GetVaccinesByAnimalId([FromRoute] Guid animalId, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        return Ok(await Mediator.Send(new GetAllVaccinesQuery(animalId), ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdVaccine([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVaccineQuery(id)));
    }

    [Authorize]
    [HttpPost("me/animals/{animalId:guid}/vaccines")]
    public async Task<IActionResult> CreateVaccine([FromRoute] Guid animalId, [FromBody] VaccineInfoDto dto, CancellationToken ct)
    {
        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        var command = new CreateVaccineCommand(
            animalId,
            dto.Name,
            dto.Date,
            dto.Description,
            dto.Manufacturer
        );

        var id = await Mediator.Send(command, ct);
        return Ok(id);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVaccine(Guid id, [FromBody] UpdateVaccineCommand command)
    {
        var updateCommand = new UpdateVaccineCommand(
            id,
            command.Name,
            command.Date,
            command.Description,
            command.Manufacturer
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVaccine(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVaccineCommand(id)));
    }

    //////////////////////////////////////////////////////////////
    ////////////////////////////VACCINES//////////////////////////
    //////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////
    ////////////////////////////ProductsUsed//////////////////////
    //////////////////////////////////////////////////////////////
    [Authorize]
    [HttpGet("me/animals/products-used")]
    public async Task<IActionResult> GetAllProductsUsed([FromQuery] GetAllProductsUsedQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [Authorize]
    [HttpGet("me/animals/{animalId:guid}/products-used")]
    public async Task<IActionResult> GetProductsUsedByAnimalId([FromRoute] Guid animalId, CancellationToken ct)
    {

        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        return Ok(await Mediator.Send(new GetAllProductsUsedQuery(animalId), ct));
    }

    [Authorize]
    [HttpPost("me/animals/{animalId:guid}/products-used")]
    public async Task<IActionResult> CreateProductUsed([FromRoute] Guid animalId, [FromBody] ProductUsedInfoDto dto, CancellationToken ct)
    {

        var userId = CurrentUser.GetUserId(User);
        var myAnimals = await Mediator.Send(new GetMyAnimalsQuery(userId), ct);
        if (!myAnimals.Any(a => a.Id == animalId))
            return Forbid();

        var command = new CreateProductUsedCommand(
            animalId,
            dto.Dosage,
            dto.TimesPerDay
        );

        var id = await Mediator.Send(command, ct);
        return Ok(id);
    }
}