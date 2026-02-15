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
    public record GetAdmissionsByPatientIdQuery(string PatientId) : IRequest<List<PatientAdmissionDto>>;

    public class GetAdmissionsByPatientIdQueryHandler : IRequestHandler<GetAdmissionsByPatientIdQuery, List<PatientAdmissionDto>>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetAdmissionsByPatientIdQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<List<PatientAdmissionDto>> Handle(GetAdmissionsByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var admissions = await _admissionRepository.GetAdmissionsByPatientIdAsync(request.PatientId);
            return _mapper.Map<List<PatientAdmissionDto>>(admissions);
        }
    }
}
