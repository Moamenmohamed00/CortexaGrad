using AutoMapper;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using MediatR;
using AdmissionEntity = Cortexa.Domain.Entities.Core.Admission;

namespace Cortexa.Application.Features.Patients.Commands
{
    public class AdmitPatientCommandHandler : IRequestHandler<AdmitPatientCommand, PatientAdmissionDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdmitPatientCommandHandler(
            IPatientRepository patientRepository,
            IAdmissionRepository admissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _admissionRepository = admissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientAdmissionDto> Handle(AdmitPatientCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Street,
                request.City,
                request.State,
                request.ZipCode ?? string.Empty,
                request.Country ?? request.State);

            var patient = new Patient
            {
                // filenumber ‰⁄„·Â »⁄œÌ‰ Ì“Ìœ ·ÊÕœÂ
                Name = request.Name,
                FileNumber = request.NationalId,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Email = request.Email ?? string.Empty,
                PhoneNumber = string.IsNullOrEmpty(request.Phone) ? null :  request.Phone ,
                Address = address,
                BloodType = request.BloodType,
                DiagnosisSummary = request.DiagnosisSummary,
                NationalId= request.NationalId,
            };

            await _patientRepository.AddAsync(patient, cancellationToken);

            var admission = new AdmissionEntity
            {
                PatientId = patient.Id,
                Patient = patient,
                DoctorId = request.DoctorId,
                AdmissionDate = DateTime.UtcNow,
                InitialDiagnosis = request.InitialDiagnosis,
                Status = AdmissionStatus.Active,
                BedId = request.BedId,
                RoomId = request.RoomId
            };

            await _admissionRepository.AddAsync(admission, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PatientAdmissionDto>(patient);
            _mapper.Map(admission, dto);
            return dto;
        }
    }
}
