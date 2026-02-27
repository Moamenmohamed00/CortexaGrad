using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddPhysicalExaminationCommand : IRequest<string>
    {
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

        public string AdmissionId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
    }

    public class AddPhysicalExaminationCommandHandler
        : IRequestHandler<AddPhysicalExaminationCommand, string>
    {
        private readonly IPhysicalExaminationRepository _repo;
        private readonly IUnitOfWork _uow;

        public AddPhysicalExaminationCommandHandler(
            IPhysicalExaminationRepository repo,
            IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<string> Handle(
            AddPhysicalExaminationCommand request,
            CancellationToken ct)
        {
            var entity = new PhysicalExamination
            {
                ExamDate = request.ExamDate,
                Temperature = request.Temperature,
                BloodPressure = request.BloodPressure,
                Pulse = request.Pulse,
                RespRate = request.RespRate,
                EyeStatus = request.EyeStatus,
                SkinStatus = request.SkinStatus,
                LipsStatus = request.LipsStatus,
                HeartExam = request.HeartExam,
                AbdomenExam = request.AbdomenExam,
                LocalExamination = request.LocalExamination,
                AdmissionId = request.AdmissionId,
                DoctorId = request.DoctorId
            };

            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
