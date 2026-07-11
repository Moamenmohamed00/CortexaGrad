using MediatR;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Commands
{
    public class UpdatePatientCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string? DiagnosisSummary { get; set; }
        public BloodType? BloodType { get; set; }

        public string NationalId { get; set; } = string.Empty;
    }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            
            var patient = await _patientRepository.GetByIdAsync(request.Id);
            if (patient == null) return false;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                patient.PhoneNumber=   request.PhoneNumber ;
            }

            if (!string.IsNullOrEmpty(request.Street)||!string.IsNullOrEmpty(request.City)||!string.IsNullOrEmpty(request.State)||!string.IsNullOrEmpty(request.Country)||!string.IsNullOrEmpty(request.ZipCode))
            {
                patient.Address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            }


            if (!string.IsNullOrEmpty(request.DiagnosisSummary))
            {
                patient.DiagnosisSummary = request.DiagnosisSummary;
            }


            if (!string.IsNullOrEmpty(request.NationalId))
            {
                patient.NationalId = request.NationalId;
            }


            if (request.BloodType.HasValue)
            {
                patient.BloodType = request.BloodType.Value;
            }

            await _patientRepository.UpdateAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
