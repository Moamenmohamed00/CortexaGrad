using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Domain.Exceptions
{
    public class PatientAlreadyAdmittedException : Exception
    {
        public PatientAlreadyAdmittedException(string id, string name)
            : base($"Patient with ID {id} and Name {name} is already admitted.")
        {
        }
    }
}
