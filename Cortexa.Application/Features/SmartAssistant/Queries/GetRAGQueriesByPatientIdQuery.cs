using AutoMapper;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.SmartAssistant.Queries
{
    public record GetRAGQueriesByPatientIdQuery(string PatientId, int PageNumber = 1, int PageSize = 10) : IRequest<(List<RagQueryDto> Items, int TotalCount)>;

    public class GetRAGQueriesByPatientIdQueryHandler : IRequestHandler<GetRAGQueriesByPatientIdQuery, (List<RagQueryDto> Items, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRAGQueriesByPatientIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(List<RagQueryDto> Items, int TotalCount)> Handle(GetRAGQueriesByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _unitOfWork.Rags.GetPaginatedByPatientIdAsync(request.PatientId, request.PageNumber, request.PageSize, cancellationToken);
            
            var dtos = _mapper.Map<List<RagQueryDto>>(items);
            return (dtos, totalCount);
        }
    }
}
