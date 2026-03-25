using AutoMapper;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Exceptions;
using Cortexa.Domain.ValueObjects;
using MediatR;
using AdmissionEntity = Cortexa.Domain.Entities.Core.Admission;

namespace Cortexa.Application.Features.Patients.Commands
{
    public record AdmitPatientCommand(
      string Name,
      string NationalId,
      DateTime DateOfBirth,
      Gender Gender,
      string? Email,
      string? Phone,
      string Street,
      string City,
      string State,
      string? ZipCode,
      string? Country,
      BloodType BloodType,
      string? DiagnosisSummary,
      string DoctorId,
      string InitialDiagnosis,
      string? BedId,
      string? RoomId
  ) : IRequest<PatientAdmissionDto>;
    public class AdmitPatientCommandHandler : IRequestHandler<AdmitPatientCommand, PatientAdmissionDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdmitPatientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientAdmissionDto> Handle(AdmitPatientCommand request, CancellationToken cancellationToken)
        {
            // 1. التأكد من حالة السرير أولاً
            Bed? bed = null;
            if (!string.IsNullOrEmpty(request.BedId))
            {
                bed = await _unitOfWork.Beds.GetByIdAsync(request.BedId)
                    ?? throw new KeyNotFoundException($"Bed with ID '{request.BedId}' was not found.");

                if (bed.Status != BedStatus.Available)
                    throw new BedNotAvailableException(bed.Id, bed.BedNumber, bed.RoomId);
            }

            // 2. البحث عن المريض بالرقم القومي (بدلاً من الإضافة المباشرة)
            var patient = await _unitOfWork.Patients.GetByNationalIdAsync(request.NationalId, cancellationToken);

            if (patient == null)
            {
                // مريض جديد: أنشئ الكائن وأضفه
                var address = new Address(
                    request.Street,
                    request.City,
                    request.State,
                    request.ZipCode ?? string.Empty,
                    request.Country ?? request.State);

                patient = new Patient
                {
                    Name = request.Name,
                    FileNumber = request.NationalId,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Email = request.Email ?? string.Empty,
                    PhoneNumber = request.Phone,
                    Address = address,
                    BloodType = request.BloodType,
                    DiagnosisSummary = request.DiagnosisSummary,
                    NationalId = request.NationalId,
                };

                await _unitOfWork.Patients.AddAsync(patient, cancellationToken);
            }
            else
            {
                // مريض موجود مسبقاً: يمكنك تحديث بياناته هنا إذا لزم الأمر
                patient.PhoneNumber = request.Phone ?? patient.PhoneNumber;
                await _unitOfWork.Patients.UpdateAsync(patient);
            }

            // 3. إنشاء الـ Admission (سواء للمريض الجديد أو الموجود)
            var admission = new AdmissionEntity
            {
                PatientId = patient.Id, // هنا نستخدم الـ Id سواء كان جديداً أو قديماً
                DoctorId = request.DoctorId,
                AdmissionDate = DateTime.UtcNow,
                InitialDiagnosis = request.InitialDiagnosis,
                Status = AdmissionStatus.Active,
                BedId = request.BedId,
                RoomId = request.RoomId
            };

            await _unitOfWork.Admissions.AddAsync(admission, cancellationToken);

            // 4. تحديث حالة السرير
            if (bed != null)
            {
                bed.Status = BedStatus.Occupied;
                bed.CurrentAdmissionId = admission.Id;
                await _unitOfWork.Beds.UpdateAsync(bed);
            }

            // 5. حفظ كل التغييرات في Transaction واحد
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. التحويل إلى DTO
            var dto = _mapper.Map<PatientAdmissionDto>(admission);
            //_mapper.Map(admission, dto);

            return dto;
        }
    }
}

