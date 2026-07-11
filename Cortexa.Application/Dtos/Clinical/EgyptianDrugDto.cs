using System.Text.Json.Serialization;

namespace Cortexa.Application.Dtos.Clinical;

public class EgyptianDrugDto
{
    [JsonPropertyName("commercial_name_en")]
    public string CommercialNameEn { get; set; } = string.Empty;

    [JsonPropertyName("scientific_name")]
    public string ScientificName { get; set; } = string.Empty;

    [JsonPropertyName("drug_class")]//kind of drug(vitamen,cold....)
    public string DrugClass { get; set; } = string.Empty;

    [JsonPropertyName("route")]//how drug taken
    public string Route { get; set; } = string.Empty;
}