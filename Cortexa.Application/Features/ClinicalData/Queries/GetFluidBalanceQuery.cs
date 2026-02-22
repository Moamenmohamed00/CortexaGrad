using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetFluidBalanceQuery(string AdmissionId) : IRequest<List<FluidBalanceDto>>;

    public class GetFluidBalanceQueryHandler : IRequestHandler<GetFluidBalanceQuery, List<FluidBalanceDto>>
    {
        private readonly IClinicalRepository _clinicalRepository;
        private readonly IMapper _mapper;

        public GetFluidBalanceQueryHandler(IClinicalRepository clinicalRepository, IMapper mapper)
        {
            _clinicalRepository = clinicalRepository;
            _mapper = mapper;
        }

        public async Task<List<FluidBalanceDto>> Handle(GetFluidBalanceQuery request, CancellationToken cancellationToken)
        {
            var fluidBalances = await _clinicalRepository.GetFluidBalanceByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<FluidBalanceDto>>(fluidBalances);
        }
    }
}
