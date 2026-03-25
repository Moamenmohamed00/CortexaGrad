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
    public record GetInterventionProcedureQuery(string AdmissionId)
     : IRequest<List<InterventionProcedureDto>>;

    public class GetInterventionProcedureQueryHandler
        : IRequestHandler<GetInterventionProcedureQuery, List<InterventionProcedureDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetInterventionProcedureQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InterventionProcedureDto>> Handle(
            GetInterventionProcedureQuery request,
            CancellationToken ct)
        {
            var data = await _unitofwork.InterventionProcedures.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<InterventionProcedureDto>>(data);
        }
    }
}
