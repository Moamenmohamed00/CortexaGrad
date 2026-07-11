using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdatePhysicalExaminationCommand : IRequest<bool>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public DateTime ExamDate { get; set; }
        public float Temperature { get; set; }
        public string BloodPressure { get; set; } = string.Empty;
        public int Pulse { get; set; }
        public int RespRate { get; set; }
        public string EyeStatus { get; set; } = string.Empty;
        public string SkinStatus { get; set; } = string.Empty;
        public string LipsStatus { get; set; } = string.Empty;
        public string HeartExam { get; set; } = string.Empty;
        public string AbdomenExam { get; set; } = string.Empty;
        public string LocalExamination { get; set; } = string.Empty;
    }
    public class UpdatePhysicalExaminationCommandHandler : IRequestHandler<UpdatePhysicalExaminationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePhysicalExaminationCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdatePhysicalExaminationCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.PhysicalExaminations.GetByIdAsync(request.Id);
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            entity.ExamDate = request.ExamDate;
            entity.Temperature = request.Temperature;
            entity.BloodPressure = request.BloodPressure;
            entity.Pulse = request.Pulse;
            entity.RespRate = request.RespRate;
            entity.EyeStatus = request.EyeStatus;
            entity.SkinStatus = request.SkinStatus;
            entity.LipsStatus = request.LipsStatus;
            entity.HeartExam = request.HeartExam;
            entity.AbdomenExam = request.AbdomenExam;
            entity.LocalExamination = request.LocalExamination;

            await _unitOfWork.PhysicalExaminations.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

   
}

