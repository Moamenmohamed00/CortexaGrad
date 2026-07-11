using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Application.Models.Dashboard;
using Cortexa.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Actions;

namespace Cortexa.Infrastructure.Services
{
    public record BedStatistics
    {
        public int Total { get; init; }
        public int Occupied { get; init; }
    }

    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IDoctorRepository _doctorRepository; 
        private readonly IBedRepository _bedRepository;
        private readonly INurseRepository _nurseRepository;
        public AdmissionService(IAdmissionRepository admissionRepository, IDoctorRepository doctorRepository, IBedRepository bedRepository, INurseRepository nurseRepository)
        {
            _admissionRepository = admissionRepository;
            _doctorRepository = doctorRepository;
            _bedRepository = bedRepository;
            _nurseRepository = nurseRepository;
        }
        public async Task<HospitalOperationsModel> GetHospitalOperationsAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // جلب الإحصائيات الأساسية باستعلامات منفصلة برمجياً لكنها ستنفذ بكفاءة
            var ActivePatients = await _admissionRepository.GetActiveAdmissionsAsync(cancellationToken);
            var TotalActivePatients = ActivePatients.Count;

            var admissionsToday = await _admissionRepository.GetAdmissionsByDateAsync(today, cancellationToken);
            var totalAdmissionsToday = admissionsToday.Count;

            var dischargesToday = await _admissionRepository.GetDischargesByDateAsync(today, cancellationToken);
            var totalDischargesToday = dischargesToday.Count;


            // حساب نسبة إشغال الأسرة: (الأسرة المشغولة / العدد الكلي) * 100
            var bedStats = await GetBedStatisticsAsync(cancellationToken);

            decimal occupancyPercentage = 0;
            if (bedStats != null && bedStats.Total > 0)
            {
                occupancyPercentage = Math.Round(((decimal)bedStats.Occupied / bedStats.Total) * 100, 2);
            }

            // حساب الطاقم الطبي النشط حالياً بناءً على شفت اليوم (كمثال)
            // يمكنك تعديل المنطق بناءً على طريقة تخزين الشفتات لديك
            var activeStaff = await _doctorRepository.CountActiveDoctorsTodayAsync(today, cancellationToken);
                activeStaff += await _nurseRepository.CountActiveNursesTodayAsync(today, cancellationToken);

            return new HospitalOperationsModel(
                TotalActivePatients: TotalActivePatients,
                AdmissionsToday: totalAdmissionsToday,
                DischargesToday: totalDischargesToday,
                BedOccupancyPercentage: occupancyPercentage,
                ActiveStaffOnShift: activeStaff
            );
        }

        public async Task<string> GetPatientNameByAdmissionIdAsync(string admissionId, CancellationToken cancellationToken)
        {
            var admission = await _admissionRepository.GetByIdWithPatientDataAsync(admissionId, cancellationToken);
            if (admission == null)
            {
                return "Unknown Patient";
            }
            return admission?.Patient?.Name ?? "Unknown Patient";
        }
        public Task TransferPatientAsync(string admissionId, string newBedId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        private async Task<BedStatistics> GetBedStatisticsAsync(CancellationToken cancellationToken)
        {
            var totalBeds = await _bedRepository.GetAllAsync();
            var occupiedBeds = await _bedRepository.GetOccupiedBedsAsync(cancellationToken);
            return new BedStatistics
            {
                Total = totalBeds.Count,
                Occupied = occupiedBeds.Count
            };
        }
    }
}
