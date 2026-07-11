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
    public record GetNursingNotesQuery(string AdmissionId)
    : IRequest<List<NursingNotesDto>>;

    public class GetNursingNotesQueryHandler
        : IRequestHandler<GetNursingNotesQuery, List<NursingNotesDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetNursingNotesQueryHandler(
            IUnitOfWork unitofwork,
            IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        public async Task<List<NursingNotesDto>> Handle(
            GetNursingNotesQuery request,
            CancellationToken ct)
        {
            var data = await _unitofwork.NursingNotes.GetByAdmissionIdAsync(request.AdmissionId);

            
            return _mapper.Map<List<NursingNotesDto>>(data);
        }
    }
}
