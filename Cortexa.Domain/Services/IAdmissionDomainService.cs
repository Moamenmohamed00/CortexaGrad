//using Cortexa.Domain.Entities.Actors;
//using Cortexa.Domain.Entities.Core;
//using Cortexa.Domain.Entities.Infrastructure;
//using Cortexa.Domain.Enums;


//namespace Cortexa.Domain.Services
//{
//    public interface IAdmissionDomainService
//    {
//        Task<Admission> PerformAdmissionAsync(
//            Patient patient,
//            string doctorId,
//            string? bedId,
//            string? roomId,
//            string initialDiagnosis,
//            CancellationToken ct);
//    }

//    public class AdmissionDomainService : IAdmissionDomainService
//    {
//        private readonly IBedRepository _bedRepository;

//        public AdmissionDomainService(IBedRepository bedRepository)
//        {
//            _bedRepository = bedRepository;
//        }

//        public async Task<Admission> PerformAdmissionAsync(
//            Patient patient, string doctorId, string? bedId, string? roomId, string initialDiagnosis, CancellationToken ct)
//        {
//            Bed? bed = null;
//            if (!string.IsNullOrEmpty(bedId))
//            {
//                bed = await _bedRepository.GetByIdAsync(bedId)
//                    ?? throw new KeyNotFoundException($"Bed {bedId} not found.");

//                if (bed.Status != BedStatus.Available)
//                    throw new Exception("Bed is already occupied."); // استخدام Exception مخصص هنا أفضل
//            }

//            var admission = new Admission
//            {
//                PatientId = patient.Id,
//                Patient = patient,
//                DoctorId = doctorId,
//                AdmissionDate = DateTime.UtcNow,
//                InitialDiagnosis = initialDiagnosis,
//                Status = AdmissionStatus.Active,
//                BedId = bedId,
//                RoomId = roomId
//            };

//            if (bed != null)
//            {
//                bed.Status = BedStatus.Occupied;
//                bed.CurrentAdmissionId = admission.Id;
//                await _bedRepository.UpdateAsync(bed);
//            }

//            return admission;
//        }
//    }
//}
