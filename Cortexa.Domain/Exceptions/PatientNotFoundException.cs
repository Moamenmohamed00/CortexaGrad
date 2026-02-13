namespace Cortexa.Domain.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException()
            : base("Patient not found.")
        {
        }
    }
}
