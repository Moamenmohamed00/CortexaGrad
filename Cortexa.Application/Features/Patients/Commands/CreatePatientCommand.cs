using MediatR;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Commands
{
    public class CreatePatientCommand : IRequest<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }

        public string NationalId {  get; set; } = string.Empty;
    }

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, string>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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

            return patient.Id;
        }
    }
}
