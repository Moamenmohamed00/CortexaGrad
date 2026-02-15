using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetPatientDetailsQuery(string PatientId) : IRequest<PatientDetailsDto>;

    public class PatientDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = new();
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? DiagnosisSummary { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public List<AdmissionDto> Admissions { get; set; } = new();
    }

    public class GetPatientDetailsQueryHandler : IRequestHandler<GetPatientDetailsQuery, PatientDetailsDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetPatientDetailsQueryHandler(
            IPatientRepository patientRepository,
            IAdmissionRepository admissionRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<PatientDetailsDto> Handle(GetPatientDetailsQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID '{request.PatientId}' was not found.");

            var admissions = await _admissionRepository.GetAdmissionsByPatientIdAsync(request.PatientId);

            var dto = new PatientDetailsDto
            {
                Id = patient.Id,
                Name = patient.Name,
                FileNumber = patient.FileNumber,
                Email = patient.Email,
                PhoneNumbers = patient.PhoneNumbers,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender.ToString(),
                DiagnosisSummary = patient.DiagnosisSummary,
                BloodType = patient.BloodType.ToString(),
                Admissions = _mapper.Map<List<AdmissionDto>>(admissions)
            };

            return dto;
        }
    }
}
