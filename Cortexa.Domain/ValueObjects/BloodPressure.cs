namespace Cortexa.Domain.ValueObjects
{
    public class BloodPressure
    {
        public int Systolic { get; private set; }
        public int Diastolic { get; private set; }

        private BloodPressure() { }

        public BloodPressure(int systolic, int diastolic)
        {
            Systolic = systolic;
            Diastolic = diastolic;
        }
    }
}
