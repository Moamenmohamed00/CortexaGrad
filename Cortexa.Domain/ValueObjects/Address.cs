using System;

namespace Cortexa.Domain.ValueObjects
{
    public class Address 
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string? ZipCode { get; private set; }

        public Address(string street, string city, string state, string zipCode, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be null or empty.", nameof(street));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be null or empty.", nameof(city));
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("State cannot be null or empty.", nameof(state));

            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode ?? string.Empty;
        }

        private Address() // For EF Core
        {
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            ZipCode = null;
        }

        public override string ToString() => $"{Street}, {City}, {State} {ZipCode}";
    }
}
