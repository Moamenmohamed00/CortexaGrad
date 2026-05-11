using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Admin
{

    // Cortexa.Application/Dtos/Admin/DashboardSummaryDto.cs
    public class DashboardSummaryDto
    {
        public int TotalActivePatients { get; set; }
        public double BedOccupancyPercentage { get; set; }
        public int HighRiskAlertsCount { get; set; }
        public int TotalRAGQueriesToday { get; set; }
        public List<RecentActivityDto> RecentSystemActivities { get; set; }
        //public List<BedStatusDto> ICUSectionStatus { get; set; }
    }

    public class RecentActivityDto
    {
        public string Action { get; set; }
        public string? UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string EntityName { get; set; }
    }
}
