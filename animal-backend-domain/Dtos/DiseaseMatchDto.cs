namespace animal_backend_domain.Dtos;

public class DiseaseMatchDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string LatinName { get; set; }
	public string Category { get; set; }
	public string Description { get; set; }
	public int MatchCount { get; set; }
	public double MatchPercentage { get; set; }
	public List<string> MatchedSymptoms { get; set; } = [];
}