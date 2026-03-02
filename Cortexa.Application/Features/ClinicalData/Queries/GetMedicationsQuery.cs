using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
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
        private readonly IMedicationRepository _repo;
        private readonly IMapper _mapper;

        public GetMedicationsQueryHandler(
            IMedicationRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<MedicationDto>> Handle(
            GetMedicationsQuery request,
            CancellationToken ct)
        {
            var data = await _repo.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<MedicationDto>>(data);
        }
    }
}
