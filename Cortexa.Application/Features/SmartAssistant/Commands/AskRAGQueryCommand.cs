using AutoMapper;
using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Entities.AI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.SmartAssistant.Commands
{
    public record AskRAGQueryCommand(
        string ProjectId, 
        string AdmissionId, 
        string QueryText, 
        int Limit = 5
    ) : IRequest<RagQueryDto>;

    public class AskRAGQueryCommandHandler : IRequestHandler<AskRAGQueryCommand, RagQueryDto>
    {
        private readonly IAIService _aiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AskRAGQueryCommandHandler(IAIService aiService, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _aiService = aiService;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<RagQueryDto> Handle(AskRAGQueryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.QueryText))
                throw new ArgumentException("Query text cannot be empty", nameof(request.QueryText));
            if (string.IsNullOrWhiteSpace(request.ProjectId))
                throw new ArgumentException("Project ID cannot be empty", nameof(request.ProjectId));

            string currentUserEmail = _currentUserService.UserEmail 
                ?? throw new InvalidOperationException("Current user email is not available");

            var Doctor = await _unitOfWork.Doctors.GetByEmailAsync(currentUserEmail)
                ?? throw new KeyNotFoundException("Doctor not found for the current user");

            // 1. Fetch patient side from admission
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId)
                ?? throw new KeyNotFoundException("Admission res not found");

            // 2. Call the Python AI service
            var result = await _aiService.AskQuestionAsync(request.ProjectId, request.AdmissionId, request.QueryText, request.Limit, cancellationToken);
            
            // 3. Prepare the entity to save chat history
            var ragEntity = new RAGQuery
            {
                QueryText = request.QueryText,
                GeneratedResponse = result.Answer ?? "No response obtained",
                DoctorId = Doctor.Id,
                PatientId = admission.PatientId,
                QueryDateTime = DateTime.UtcNow,
                ScoreTrust = 0.9f, 
                RelevanceLevel = Cortexa.Domain.Enums.RelevanceLevel.High
            };

            await _unitOfWork.Rags.AddAsync(ragEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Return the new DTO mapped, appending the sources so UI can see them
            var mapped = _mapper.Map<RagQueryDto>(ragEntity);
            return mapped with { Sources = result.Sources ?? new List<string>() };
        }
    }
}
