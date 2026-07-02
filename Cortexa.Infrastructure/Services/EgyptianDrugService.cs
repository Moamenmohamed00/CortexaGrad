using System.Text.Json;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Services;

namespace Cortexa.Infrastructure.Services;

public class EgyptianDrugService : IEgyptianDrugService
{
    private readonly List<EgyptianDrugDto> _drugs;

    public EgyptianDrugService()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "cleaned_egyptian_drugs.json");
        
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _drugs = JsonSerializer.Deserialize<List<EgyptianDrugDto>>(json) ?? new List<EgyptianDrugDto>();
        }
        else
        {
            _drugs = new List<EgyptianDrugDto>();
        }
    }

    public IEnumerable<EgyptianDrugDto> SearchDrugs(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return _drugs.Take(20);

        return _drugs.Where(d => 
           // (!string.IsNullOrEmpty(d.ScientificName) && d.ScientificName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(d.CommercialNameEn) && d.CommercialNameEn.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .Take(20); //20 is the number of drugs to be returned
    }
}