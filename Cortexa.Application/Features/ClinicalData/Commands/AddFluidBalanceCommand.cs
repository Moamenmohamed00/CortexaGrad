using MediatR;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Repositories.Clinical;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddFluidBalanceCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public FluidBalanceCategory Category { get; set; }
        public FluidType Type { get; set; }
        public int Amount_ML { get; set; }
        public DateTime RecordedAt { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }
    /*
     * command
     * add casehistory 
     * add PhysicalExaminations
     * add InterventionProcedure
     * Queries
     * get nursing notes
     * get medication 
     * get case history
     * get physical examination
     * get intervention procedure
     */
    //public class AddCaseHistoryCommand : IRequest<string>
    //{
    //    public string AdmissionId { get; set; } = string.Empty;
    //    public string Complaint { get; set; } = string.Empty;
    //    public string PresentIllness { get; set; } = string.Empty;
    //    public string? ChronicDisease { get; set; } // Could be List<string> for future
    //    public string? GeneticDisease { get; set; }
    //    public string? MaritalHistory { get; set; }
    //    public string? SpecialHabits { get; set; }
    //    public string? ClinicalNotes { get; set; }
    //    public string DoctorId { get; set; } = string.Empty;
    //}
    //public class AddCaseHistoryCommandHandler
    //: IRequestHandler<AddCaseHistoryCommand, string>
    //{
    //    private readonly ICaseHistoryRepository _caseHistory;
    //    private readonly IUnitOfWork _unitOfWork;

    //    public AddCaseHistoryCommandHandler(
    //        ICaseHistoryRepository caseHistory,
    //        IUnitOfWork unitOfWork)
    //    {
    //        _caseHistory = caseHistory;
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<string> Handle(
    //        AddCaseHistoryCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var entity = new CaseHistory
    //        {
    //            AdmissionId = request.AdmissionId,
    //            Complaint = request.Complaint,
    //            PresentIllness = request.PresentIllness,
    //            ChronicDisease = request.ChronicDisease,
    //            GeneticDisease = request.GeneticDisease,
    //            MaritalHistory = request.MaritalHistory,
    //            SpecialHabits = request.SpecialHabits,
    //            ClinicalNotes = request.ClinicalNotes,
    //            DoctorId = request.DoctorId
    //        };

    //        await _caseHistory.AddAsync(entity, cancellationToken);

    //        await _unitOfWork.SaveChangesAsync(cancellationToken);

    //        return entity.Id;
    //    }
    //}

    public class AddFluidBalanceCommandHandler : IRequestHandler<AddFluidBalanceCommand, string>
    {
        private readonly  IFluidBalanceRepository _fluidBalanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFluidBalanceCommandHandler( IFluidBalanceRepository fluidBalanceRepository, IUnitOfWork unitOfWork)
        {
            _fluidBalanceRepository = fluidBalanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddFluidBalanceCommand request, CancellationToken cancellationToken)
        {
            var entity = new FluidBalance
            {
                AdmissionId = request.AdmissionId,
                Category = request.Category,
                Type = request.Type,
                Amount_ML = request.Amount_ML,
                RecordedAt = request.RecordedAt,
                NurseId = request.NurseId
            };

            await _fluidBalanceRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
