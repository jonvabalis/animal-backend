using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_domain.Entities;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class FindDiseasesBySymptomsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<FindDiseasesBySymptomsQuery, FindDiseasesBySymptomsResponse>
{
    public async Task<FindDiseasesBySymptomsResponse> Handle(FindDiseasesBySymptomsQuery request, CancellationToken cancellationToken)
    {
        if (request.Symptoms.Count == 0)
        {
            return new FindDiseasesBySymptomsResponse
            {
                Success = false,
                Message = "Nepateikta jokių simptomų"
            };
        }

        var normalizedSymptoms = request.Symptoms
            .Select(s => s.Trim().ToLowerInvariant())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        if (normalizedSymptoms.Count == 0)
        {
            return new FindDiseasesBySymptomsResponse
            {
                Success = false,
                Message = "Nepateikta jokių galiojančių simptomų"
            };
        }

        var allDiseases = await dbContext.Diseases.ToListAsync(cancellationToken);

        var matches = new List<DiseaseMatchDto>();

        foreach (var disease in allDiseases)
        {
            var match = CalculateMatch(disease, normalizedSymptoms);
            
            if (match.MatchCount > 0)
            {
                matches.Add(match);
            }
        }

        var sortedMatches = matches
            .OrderByDescending(m => m.MatchCount)
            .ThenByDescending(m => m.MatchPercentage)
            .Take(request.MaxResults)
            .ToList();

        return new FindDiseasesBySymptomsResponse
        {
            Success = true,
            Message = $"Rasta {sortedMatches.Count} ligų, atitinkančių simptomus",
            Matches = sortedMatches
        };
    }

    private DiseaseMatchDto CalculateMatch(Disease disease, List<string> symptoms)
    {
        var descriptionLower = disease.Description.ToLowerInvariant();
        var matchedSymptoms = new List<string>();
        var matchCount = 0;

        foreach (var symptom in symptoms)
        {
            if (ContainsSymptom(descriptionLower, symptom))
            {
                matchCount++;
                matchedSymptoms.Add(symptom);
            }
        }

        var matchPercentage = symptoms.Count > 0 
            ? (double)matchCount / symptoms.Count * 100 
            : 0;

        return new DiseaseMatchDto
        {
            Id = disease.Id,
            Name = disease.Name,
            LatinName = disease.LatinName,
            Category = disease.Category.ToString(),
            Description = disease.Description,
            MatchCount = matchCount,
            MatchPercentage = Math.Round(matchPercentage, 2),
            MatchedSymptoms = matchedSymptoms
        };
    }

    private bool ContainsSymptom(string description, string symptom)
    {
        var words = description.Split([' ', ',', '.', ';', ':', '!', '?', '\n', '\r'], 
            StringSplitOptions.RemoveEmptyEntries);
        
        return words.Any(word => word.Contains(symptom));
    }
}