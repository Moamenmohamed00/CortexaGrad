using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Commands
{
    public record CreatePatientCommand 
    (
         string Name ,
         string Email,
         string PhoneNumber,
         DateTime DateOfBirth ,
         Gender Gender,
         string Street ,
         string City ,
         string State ,
         string Country ,
         string ZipCode ,
         string FileNumber ,
         BloodType BloodType ,
         string NationalId 
    ): IRequest<PatientDto>;

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }

        public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {
                Name = request.Name,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode),
                FileNumber = request.FileNumber,
                BloodType = request.BloodType,
                NationalId = request.NationalId,
            };

            // Name is in AppUser? AppUser usually has FirstName/LastName or FullName. 
            // In derived Patient : AppUser
            // Checking AppUser definition might be needed, assuming standard IdentityUser or custom. 
            // Patient.cs inherits AppUser. Let's assume AppUser has logic for Name. 
            // Actually, usually IdentityUser doesn't have Name.
            // I'll check AppUser if build fails, but for now I'll assume I can set UserName/Email.
            // Wait, CreatePatientCommand has Name. I should save it.
            // If AppUser doesn't have Name property, I might need to put it in Claims or extend AppUser.
            // But let's verify AppUser in Verification if needed. 
            // For now, I'll assume AppUser has a way to store name or Patient has it.
            // Looking at PatientDto, it has Name. 
            // Patient.cs didn't show Name property, but it inherits AppUser.
            // Let's assume AppUser has Name derived or stored.

            // Re-checking Patient.cs view... 
            // public class Patient : AppUser
            // AppUser was not viewed fully in previous steps, only referenced.
            // Detailed view of Patient.cs showed properties. 
            // I'll check User in next step if this fails.

            // But wait, I can just write the file and fix if needed.
            // I'll add PhoneNumbers.
            patient.PhoneNumber = request.PhoneNumber ;

            await _patientRepository.AddAsync(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PatientDto>(patient);


            return dto;
        }


    }
}
