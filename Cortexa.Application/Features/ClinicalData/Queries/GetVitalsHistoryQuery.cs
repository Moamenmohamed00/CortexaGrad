using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetVitalsHistoryQuery(string AdmissionId) : IRequest<List<VitalSignsDto>>;

    public class GetVitalsHistoryQueryHandler : IRequestHandler<GetVitalsHistoryQuery, List<VitalSignsDto>>
    {
        private readonly IClinicalRepository _clinicalRepository;
        private readonly IMapper _mapper;

        public GetVitalsHistoryQueryHandler(IClinicalRepository clinicalRepository, IMapper mapper)
        {
            _clinicalRepository = clinicalRepository;
            _mapper = mapper;
        }

        public async Task<List<VitalSignsDto>> Handle(GetVitalsHistoryQuery request, CancellationToken cancellationToken)
        {
            var vitals = await _clinicalRepository.GetVitalSignsByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<VitalSignsDto>>(vitals);
        }
    }
}
