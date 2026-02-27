using AutoMapper;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Queries
{
    public record GetCaseHistoryQuery(string AdmissionId)
      : IRequest<List<CaseHistoryDto>>;

    public class GetCaseHistoryQueryHandler
        : IRequestHandler<GetCaseHistoryQuery, List<CaseHistoryDto>>
    {
        private readonly ICaseHistoryRepository _repo;
        private readonly IMapper _mapper;

        public GetCaseHistoryQueryHandler(
            ICaseHistoryRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CaseHistoryDto>> Handle(
            GetCaseHistoryQuery request,
            CancellationToken ct)
        {
            var data = await _repo.GetByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<CaseHistoryDto>>(data);
        }
    }
}
