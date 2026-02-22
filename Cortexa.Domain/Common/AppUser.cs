using System;
using System.Collections.Generic;
using Cortexa.Domain.ValueObjects;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Common
{
    public abstract class AppUser : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Address Address { get; set; } = null!; // EF Core will set this
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string NationalId { get; set; }//validation: unique, required, max length 20

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        protected AppUser()
        {
        }
    }
}
