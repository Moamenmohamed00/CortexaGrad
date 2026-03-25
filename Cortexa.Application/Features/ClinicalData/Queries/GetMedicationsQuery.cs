using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetMedicationsQuery(string AdmissionId)
    : IRequest<List<MedicationDto>>;

    public class GetMedicationsQueryHandler
        : IRequestHandler<GetMedicationsQuery, List<MedicationDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetMedicationsQueryHandler(
            IUnitOfWork unitofwork,
            IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        public async Task<List<MedicationDto>> Handle(
            GetMedicationsQuery request,
            CancellationToken ct)
        {
            var data = await _unitofwork.Medications.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<MedicationDto>>(data);
        }
    }
}
