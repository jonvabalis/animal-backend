using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Queries;

public class FindDiseasesBySymptomsQuery : IRequest<FindDiseasesBySymptomsResponse>
{
	public List<string> Symptoms { get; set; } = new();
	
	[System.Text.Json.Serialization.JsonIgnore]
	public int MaxResults { get; set; } = 1;
}
