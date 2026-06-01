using Cortexa.Domain.Entities.Actors;
using System;
using System.Collections.Generic;
using System.Text;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.StaffSchedule
{
    public class DoctorSchedule : BaseEntity
    {
        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!;

        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }

        // To handle doctors who are "On-Call" outside regular hours
        public bool IsOnCall { get; set; }

        // Schedule status: Scheduled, Present, OnLeave, Absent
        public ScheduleStatus Status { get; set; }
    }


}
