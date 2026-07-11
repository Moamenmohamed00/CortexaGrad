using Cortexa.Application.Dtos.Clinical;

namespace Cortexa.Application.Interfaces.Services;

public interface IEgyptianDrugService
{
    IEnumerable<EgyptianDrugDto> SearchDrugs(string? searchTerm);
}