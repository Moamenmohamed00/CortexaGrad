using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Admission.Queries
{
    public record GetActiveAdmissionsQuery : IRequest<List<PatientAdmissionDto>>;

    public class GetActiveAdmissionsQueryHandler : IRequestHandler<GetActiveAdmissionsQuery, List<PatientAdmissionDto>>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetActiveAdmissionsQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<List<PatientAdmissionDto>> Handle(GetActiveAdmissionsQuery request, CancellationToken cancellationToken)
        {
            var admissions = await _admissionRepository.GetActiveAdmissionsAsync();
            return _mapper.Map<List<PatientAdmissionDto>>(admissions);
        }
    }
}
