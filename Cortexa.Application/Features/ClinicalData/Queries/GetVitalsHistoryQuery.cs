using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Repositories.Clinical;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetVitalsHistoryQuery(string AdmissionId) : IRequest<List<VitalSignsDto>>;

    public class GetVitalsHistoryQueryHandler : IRequestHandler<GetVitalsHistoryQuery, List<VitalSignsDto>>
    {
        private readonly IVitalSignsRepository _vitalSignsRepository;
        private readonly IMapper _mapper;

        public GetVitalsHistoryQueryHandler(IVitalSignsRepository vitalSignsRepository, IMapper mapper)
        {
            _vitalSignsRepository = vitalSignsRepository;
            _mapper = mapper;
        }

        public async Task<List<VitalSignsDto>> Handle(GetVitalsHistoryQuery request, CancellationToken cancellationToken)
        {
            var vitals = await _vitalSignsRepository.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<VitalSignsDto>>(vitals);
        }
    }
}
