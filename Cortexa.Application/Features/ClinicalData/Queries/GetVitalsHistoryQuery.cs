using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Dtos.Rooms;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetVitalsHistoryQuery(string AdmissionId) : IRequest<List<VitalSignsDto>>;

    public class GetVitalsHistoryQueryHandler : IRequestHandler<GetVitalsHistoryQuery, List<VitalSignsDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetVitalsHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<VitalSignsDto>> Handle(GetVitalsHistoryQuery request, CancellationToken cancellationToken)
        {
            var vitals = await _unitofwork.VitalSigns.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<VitalSignsDto>>(vitals);
        }
    }

    
}
