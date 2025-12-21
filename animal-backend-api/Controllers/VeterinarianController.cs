using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using animal_backend_domain.Dtos;
using animal_backend_domain.Entities;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace animal_backend_api.Controllers;
using ClosedXML.Excel;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.Json;

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
    public async Task<IActionResult> DownloadVeterinariansExcel([FromServices] animal_backend_infrastructure.AnimalDbContext context)
    {
        // Load veterinarians with their visits
        var veterinarians = await context.Veterinarians
            .Include(v => v.Visits)
            .ToListAsync();

        // Also load users that point to veterinarians (if any)
        var users = await context.Users.ToListAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Veterinarians");

        int row = 1;
        // We'll keep a running global total of all visit sums
        double globalTotal = 0;
        double globalDuration = 0;

        var headerRows = new List<int>();

        foreach (var vet in veterinarians)
        {   

            // Header for each veterinarian block
            ws.Cell(row, 1).Value = "Name";
            ws.Cell(row, 2).Value = "Surname";
            ws.Cell(row, 3).Value = "E-mail";
            ws.Cell(row, 4).Value = "Rank";
            ws.Cell(row, 5).Value = "Salary";

            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 2).Style.Font.Bold = true;
            ws.Cell(row, 3).Style.Font.Bold = true;
            ws.Cell(row, 4).Style.Font.Bold = true;
            ws.Cell(row, 5).Style.Font.Bold = true;

            var headerRange = ws.Range(row, 1, row, 6);
            headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;

            // remember this header row to reapply after broad border styling
            headerRows.Add(row);

            // Fill veterinarian's main row (find user linked to this vet if exists)
            var user = users.FirstOrDefault(u => u.VeterinarianId == vet.Id);
            row++;

            ws.Cell(row, 1).Value = user?.Name ?? string.Empty;
            ws.Cell(row, 2).Value = user?.Surname ?? string.Empty;
            ws.Cell(row, 3).Value = user.Email ?? string.Empty;
            ws.Cell(row, 4).Value = vet.Rank;
            ws.Cell(row, 5).Value = vet.Salary.ToString() + " €";
            

            row += 2; // leave one empty row before visits listing

            // Visits header
            ws.Cell(row, 1).Value = "Visit Start";
            ws.Cell(row, 2).Value = "Visit End";
            ws.Cell(row, 3).Value = "User (Owner)";
            ws.Cell(row, 4).Value = "Price";
            ws.Cell(row, 5).Value = "Visit type";
            ws.Cell(row, 6).Value = "Duration";
            row++;
            
            double vetSum = 0;
            double timeDuration = 0;
            var visits = vet.Visits.OrderBy(v => v.Start).ToList();
            foreach (var visit in visits)
            {
                ws.Cell(row, 1).Value = visit.Start;
                ws.Cell(row, 2).Value = visit.End;
                // Owner name if available
                var owner = users.FirstOrDefault(u => u.Id == visit.UserId);
                ws.Cell(row, 3).Value = owner != null ? $"{owner.Name} {owner.Surname}" : string.Empty;
                ws.Cell(row, 4).Value = visit.Price.ToString() + " €";
                ws.Cell(row, 5).Value = visit.Type.ToString();
                var duration = visit.End != default(DateTime) ? (visit.End - visit.Start).TotalMinutes + " min" : "N/A";
                ws.Cell(row, 6).Value = duration;
                vetSum += visit.Price;
                timeDuration += visit.End != default(DateTime) ? (visit.End - visit.Start).TotalMinutes : 0;
                row++;
            }
            ws.Cell(row, 3).Value = "Visit Count"; // empty row before subtotal
            ws.Cell(row, 4).Value = visits.Count;
            ws.Cell(row, 3).Style.Font.Bold = true;
            ws.Cell(row, 4).Style.Font.Bold = true;
            row++;
            // Vet subtotal row
            ws.Cell(row, 3).Value = "Visit Sum:";
            ws.Cell(row, 4).Value = vetSum.ToString() + " €";
            ws.Cell(row, 3).Style.Font.Bold = true;
            ws.Cell(row, 4).Style.Font.Bold = true;
            row++;
            ws.Cell(row, 4).Value = (vetSum+vet.Salary).ToString() + " €";
            ws.Cell(row, 3).Value = "Salary + Visits:";
            ws.Cell(row, 3).Style.Font.Bold = true;
            ws.Cell(row, 4).Style.Font.Bold = true;
            globalTotal += vetSum;
            ws.Cell(row, 5).Value = "Total work time:";
            ws.Cell(row, 6).Value = timeDuration.ToString() + " min";
            ws.Cell(row, 5).Style.Font.Bold = true;
            ws.Cell(row, 6).Style.Font.Bold = true;
            globalDuration += timeDuration;
            

            row += 2; // space before next veterinarian
        }

        ws.Cell(row, 1).Value = "Summary";

        ws.Cell(row, 1).Style.Font.Bold = true;
        ws.Cell(row, 3).Value = "Total work time:";
        ws.Cell(row, 4).Value = globalDuration.ToString() + " min";
        ws.Cell(row, 3).Style.Font.Bold = true;
        ws.Cell(row, 4).Style.Font.Bold = true;
        row++;
        // Grand total at the end
        ws.Cell(row, 3).Value = "Total(visits):";
        ws.Cell(row, 4).Value = globalTotal.ToString() + " €";
        ws.Cell(row, 3).Style.Font.Bold = true;
        ws.Cell(row, 4).Style.Font.Bold = true;
        row++;

        ws.Cell(row, 3).Value = "Total(salaries):";        
        ws.Cell(row, 4).Value = veterinarians.Sum(v => v.Salary).ToString() + " €";
        ws.Cell(row, 3).Style.Font.Bold = true;
        ws.Cell(row, 4).Style.Font.Bold = true;
        row++;

        ws.Cell(row, 3).Value = "Overall Total:";
        ws.Cell(row, 4).Value = (globalTotal + veterinarians.Sum(v => v.Salary)).ToString() + " €";
        ws.Cell(row, 3).Style.Font.Bold = true;
        ws.Cell(row, 4).Style.Font.Bold = true;
        
        // after writing all data, 'row' is next empty row
        var lastRow = row - 5;
        var lastCol = 6; // adjust if more/fewer cols
        var usedRange = ws.Range(1, 1, lastRow, lastCol);
        usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        foreach (var r in headerRows)
        {
            ws.Range(r, 1, r, lastCol).Style.Border.TopBorder = XLBorderStyleValues.Thick;
        }

        // Simple formatting
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;
        var bytes = ms.ToArray();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "veterinarians_with_visits.xlsx");
    }
}