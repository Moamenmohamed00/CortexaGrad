using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Queries;

// الـ Query
public record SearchEgyptianDrugsQuery(string? SearchTerm) : IRequest<ResultDto<IEnumerable<EgyptianDrugDto>>>;

// الـ Handler
public class SearchEgyptianDrugsQueryHandler : IRequestHandler<SearchEgyptianDrugsQuery, ResultDto<IEnumerable<EgyptianDrugDto>>>
{
    private readonly IEgyptianDrugService _drugService;

    public SearchEgyptianDrugsQueryHandler(IEgyptianDrugService drugService)
    {
        _drugService = drugService;
    }

    public Task<ResultDto<IEnumerable<EgyptianDrugDto>>> Handle(SearchEgyptianDrugsQuery request, CancellationToken cancellationToken)
    {
        var results = _drugService.SearchDrugs(request.SearchTerm);
        
        return Task.FromResult(ResultDto<IEnumerable<EgyptianDrugDto>>.SuccessResult(results));
    }
}
