using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetPhysicalExaminationQuery(string AdmissionId)
        : IRequest<List<PhysicalExaminationDto>>;

    public class GetPhysicalExaminationQueryHandler
        : IRequestHandler<GetPhysicalExaminationQuery, List<PhysicalExaminationDto>>
    {
        private readonly IPhysicalExaminationRepository _repo;
        private readonly IMapper _mapper;

        public GetPhysicalExaminationQueryHandler(
            IPhysicalExaminationRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<PhysicalExaminationDto>> Handle(
            GetPhysicalExaminationQuery request,
            CancellationToken ct)
        {
            var data = await _repo.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<PhysicalExaminationDto>>(data);
        }
    }
}
