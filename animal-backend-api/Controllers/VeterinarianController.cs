using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using animal_backend_domain.Dtos;
using animal_backend_domain.Entities;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace animal_backend_api.Controllers;
using ClosedXML.Excel;
using System.IO;

public class VeterinarianController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVeterinariansQuery query)
    {
        return Ok(await Mediator.Send(new GetAllVeterinariansQuery()));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVeterinarianQuery(id)));
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] VeterinarianCreateDto dto)
    {
        var command = new CreateVeterinarianCommand(
            dto.Name,
            dto.Surname,
            dto.Email,
            dto.Password,
            dto.Role,
            dto.PhoneNumber,
            dto.PhotoUrl,
            dto.BirthDate,
            dto.Rank,
            dto.Responsibilities,
            dto.Education,
            dto.Salary,
            dto.FullTime,
            dto.HireDate,
            dto.ExperienceYears,
            dto.Gender
        );
        
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVeterinarianCommand command)
    {
        var updateCommand = new UpdateVeterinarianCommand(
            id,
            command.BirthDate,
            command.Rank,
            command.Responsibilities,
            command.Education,
            command.Salary,
            command.FullTime,
            command.HireDate,
            command.ExperienceYears,
            command.Gender
        );
        return Ok(await Mediator.Send(updateCommand));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVeterinarianCommand(id)));
    }

   [HttpGet("excel")]
    public async Task<IActionResult> DownloadVeterinariansExcel()
    {
        // Fetch all veterinarians
        var veterinarians = await Mediator.Send(new GetAllVeterinariansQuery());

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Veterinarians");

        // Header row
        ws.Cell(1, 1).Value = "Name";
        ws.Cell(1, 2).Value = "Surname";
        ws.Cell(1, 3).Value = "Email";
        ws.Cell(1, 4).Value = "PhoneNumber";
        ws.Cell(1, 5).Value = "Rank";
        // Add more headers as needed

        int row = 2;
        foreach (var vet in veterinarians)
        {   
            if(vet.VeterinarianGuid == null) continue; 
            ws.Cell(row, 1).Value = vet.Name;
            ws.Cell(row, 2).Value = vet.Surname;
            ws.Cell(row, 3).Value = vet.Email;
            ws.Cell(row, 4).Value = vet.PhoneNumber;
            ws.Cell(row, 5).Value = vet.Rank;
            // Add more fields as needed
            row++;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;
        var bytes = ms.ToArray();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "veterinarians.xlsx");
    }
}