using System;

namespace Cortexa.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to assign a patient to a bed that is not available
    /// </summary>
    public class BedNotAvailableException : Exception
    {
        public string BedId { get; }
        public string? BedNumber { get; }
        public string? RoomId { get; }

        public BedNotAvailableException(string bedId)
            : base($"Bed with ID '{bedId}' is not available for assignment.")
        {
            BedId = bedId;
        }

        public BedNotAvailableException(string bedId, string bedNumber, string? roomId = null)
            : base($"Bed '{bedNumber}' (ID: {bedId}) is not available for assignment.")
        {
            BedId = bedId;
            BedNumber = bedNumber;
            RoomId = roomId;
        }

        public BedNotAvailableException(string bedId, string bedNumber, string? roomId, Exception innerException)
            : base($"Bed '{bedNumber}' (ID: {bedId}) is not available for assignment.", innerException)
        {
            BedId = bedId;
            BedNumber = bedNumber;
            RoomId = roomId;
        }
    }
}
