namespace animal_backend_domain.Dtos;

public class FindDiseasesBySymptomsResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public List<DiseaseMatchDto> Matches { get; set; } = new();
}