using AutoMapper;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetPatientAdmissionsQuery(string PatientId) : IRequest<IReadOnlyList<AdmissionDto>>;

    public class GetPatientAdmissionsQueryHandler : IRequestHandler<GetPatientAdmissionsQuery, IReadOnlyList<AdmissionDto>>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetPatientAdmissionsQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AdmissionDto>> Handle(GetPatientAdmissionsQuery request, CancellationToken cancellationToken)
        {
            var admissions = await _admissionRepository.GetAdmissionsByPatientIdAsync(request.PatientId);
            return _mapper.Map<IReadOnlyList<AdmissionDto>>(admissions);
        }
    }
}
