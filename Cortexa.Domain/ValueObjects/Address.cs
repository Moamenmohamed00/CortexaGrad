namespace Cortexa.Domain.ValueObjects
{
    public class Address
    {
        public string Value { get; private set; }

        private Address() { }

        public Address(string value)
        {
            Value = value;
        }

    }
}
