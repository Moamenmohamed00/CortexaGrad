using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Models.Dashboard
{
    public record HospitalOperationsModel(
     int TotalActivePatients,
     int AdmissionsToday,
     int DischargesToday,
     decimal BedOccupancyPercentage,
     int ActiveStaffOnShift
 );
}
