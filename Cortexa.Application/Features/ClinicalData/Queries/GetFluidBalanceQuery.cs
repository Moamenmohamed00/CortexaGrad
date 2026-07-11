using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetFluidBalanceQuery(string AdmissionId) : IRequest<List<FluidBalanceDto>>;

    public class GetFluidBalanceQueryHandler : IRequestHandler<GetFluidBalanceQuery, List<FluidBalanceDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetFluidBalanceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<FluidBalanceDto>> Handle(GetFluidBalanceQuery request, CancellationToken cancellationToken)
        {
            var fluidBalances = await _unitofwork.FluidBalances.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<FluidBalanceDto>>(fluidBalances);
        }
    }
}
