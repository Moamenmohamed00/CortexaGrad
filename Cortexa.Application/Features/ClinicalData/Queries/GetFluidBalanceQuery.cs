using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Repositories.Clinical;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetFluidBalanceQuery(string AdmissionId) : IRequest<List<FluidBalanceDto>>;

    public class GetFluidBalanceQueryHandler : IRequestHandler<GetFluidBalanceQuery, List<FluidBalanceDto>>
    {
        private readonly IFluidBalanceRepository _fluidBalanceRepository;
        private readonly IMapper _mapper;

        public GetFluidBalanceQueryHandler(IFluidBalanceRepository fluidBalanceRepository, IMapper mapper)
        {
            _fluidBalanceRepository = fluidBalanceRepository;
            _mapper = mapper;
        }

        public async Task<List<FluidBalanceDto>> Handle(GetFluidBalanceQuery request, CancellationToken cancellationToken)
        {
            var fluidBalances = await _fluidBalanceRepository.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<FluidBalanceDto>>(fluidBalances);
        }
    }
}
