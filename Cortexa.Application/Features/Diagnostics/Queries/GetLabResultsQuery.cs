using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Queries
{
    public record GetLabResultsQuery(string OrderId) : IRequest<List<LabResultDto>>;

    public class GetLabResultsQueryHandler : IRequestHandler<GetLabResultsQuery, List<LabResultDto>>
    {
        private readonly ILabRepository _labRepository;
        private readonly IMapper _mapper;

        public GetLabResultsQueryHandler(ILabRepository labRepository, IMapper mapper)
        {
            _labRepository = labRepository;
            _mapper = mapper;
        }

        public async Task<List<LabResultDto>> Handle(GetLabResultsQuery request, CancellationToken cancellationToken)
        {
            var results = await _labRepository.GetResultsByOrderIdAsync(request.OrderId);
            return _mapper.Map<List<LabResultDto>>(results);
        }
    }
}
