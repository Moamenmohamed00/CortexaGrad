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
    public record GetPhysicalExaminationQuery(string AdmissionId)
        : IRequest<List<PhysicalExaminationDto>>;

    public class GetPhysicalExaminationQueryHandler
        : IRequestHandler<GetPhysicalExaminationQuery, List<PhysicalExaminationDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetPhysicalExaminationQueryHandler(
            IUnitOfWork unitofwork,
            IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        public async Task<List<PhysicalExaminationDto>> Handle(
            GetPhysicalExaminationQuery request,
            CancellationToken ct)
        {
            var data = await _unitofwork.PhysicalExaminations.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<PhysicalExaminationDto>>(data);
        }
    }
}
