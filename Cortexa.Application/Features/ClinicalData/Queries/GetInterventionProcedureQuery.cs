using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetInterventionProcedureQuery(string AdmissionId)
     : IRequest<List<InterventionProcedureDto>>;

    public class GetInterventionProcedureQueryHandler
        : IRequestHandler<GetInterventionProcedureQuery, List<InterventionProcedureDto>>
    {
        private readonly IInterventionProcedureRepository _repo;
        private readonly IMapper _mapper;

        public GetInterventionProcedureQueryHandler(
            IInterventionProcedureRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<InterventionProcedureDto>> Handle(
            GetInterventionProcedureQuery request,
            CancellationToken ct)
        {
            var data = await _repo.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<InterventionProcedureDto>>(data);
        }
    }
}
