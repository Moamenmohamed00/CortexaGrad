namespace Cortexa.Domain.Exceptions
{
    public class BedNotAvailableException : Exception
    {
        public BedNotAvailableException()
            : base("Bed is not available.")
        {
        }
    }
    }
