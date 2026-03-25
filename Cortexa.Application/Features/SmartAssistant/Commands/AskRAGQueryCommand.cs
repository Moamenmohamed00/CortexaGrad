using AutoMapper;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Entities.AI;
using MediatR;

namespace Cortexa.Application.Features.SmartAssistant.Commands
{
    public record AskRAGQueryCommand(RagQueryDto QueryDto) : IRequest<RagQueryDto>;

    public class AskRAGQueryCommandHandler : IRequestHandler<AskRAGQueryCommand, RagQueryDto>
    {
        private readonly IAIService _aiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AskRAGQueryCommandHandler(IAIService aiService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _aiService = aiService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RagQueryDto> Handle(AskRAGQueryCommand request, CancellationToken cancellationToken)
        {
            // 1. „⁄«·Ã… «·”ƒ«· ⁄»— Œœ„… «·‹ AI
            var responseDto = await _aiService.ProcessRagQueryAsync(request.QueryDto);

            // 2.  ÕÊÌ· «·‹ DTO ≈·Ï Entity ·Õ›ŸÂ ›Ì «·”Ã· «· «—ÌŒÌ (History)
            var ragEntity = _mapper.Map<RAGQuery>(responseDto);

            await _unitOfWork.Rags.AddAsync(ragEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RagQueryDto>(ragEntity);
        }
    }
}
