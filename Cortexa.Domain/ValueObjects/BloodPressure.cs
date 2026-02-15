namespace Cortexa.Domain.ValueObjects
{
    public class BloodPressure
    {
        public int Systolic { get; init; }
        public int Diastolic { get; init; }
        private BloodPressure(int systolic, int diastolic)
        {
            Systolic = systolic;
            Diastolic = diastolic;
        }
       public static BloodPressure Create(int systolic, int diastolic)
        {
            if (systolic <= 0 || diastolic <= 0)
                throw new ArgumentException("Blood pressure values must be positive.");

            if (diastolic >= systolic)
                throw new ArgumentException("Diastolic pressure cannot be greater than or equal to Systolic pressure.");

            // أرقام حماية عشان لو الممرضة كتبت 1200 بالغلط
            if (systolic > 300 || diastolic > 200)
                throw new ArgumentException("Unrealistic blood pressure values entered.");

            return new BloodPressure(systolic, diastolic);
        }
        public static BloodPressure Parse(string bpString)
        {
            var parts = bpString.Split('/');
            if (parts.Length != 2)
                throw new FormatException("Invalid blood pressure format. Use Systolic/Diastolic (e.g., 120/80).");

            int sys = int.Parse(parts[0]);
            int dia = int.Parse(parts[1]);

            return Create(sys, dia);
        }
        public override string ToString()
        {
            return $"{Systolic}/{Diastolic}";
        }
    }
}
