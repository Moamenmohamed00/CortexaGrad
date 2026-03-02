using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Services
{
    /// <summary>
    /// Pure domain service implementing the NHS National Early Warning Score (NEWS) standard.
    /// Calculates individual parameter scores and total NEWS score with clinical risk level.
    /// </summary>
    public static class NewsCalculator
    {
        /// <summary>
        /// Calculates the full NEWS result from vital signs.
        /// </summary>
        public static (int TotalScore, NewsRiskLevel RiskLevel) Calculate(VitalSigns vitals)
        {
            int respScore = ScoreRespirationRate(vitals.RespRate);
            int spo2Score = ScoreOxygenSaturation(vitals.PulseOxy);
            int o2Score = ScoreSupplementalOxygen(vitals.SupplementalOxygen);
            int tempScore = ScoreTemperature(vitals.Temperature);
            int bpScore = ScoreSystolicBP(vitals.BP_Systolic);
            int hrScore = ScoreHeartRate(vitals.HeartRate);
            int consciousnessScore = ScoreConsciousness(vitals.ConsciousnessLevel);

            int totalScore = respScore + spo2Score + o2Score + tempScore + bpScore + hrScore + consciousnessScore;

            // Check if any single parameter scored 3 (triggers Medium risk even if total < 5)
            bool hasExtremeSingle = respScore == 3 || spo2Score == 3 || o2Score == 3 ||
                                    tempScore == 3 || bpScore == 3 || hrScore == 3 ||
                                    consciousnessScore == 3;

            var riskLevel = DetermineRiskLevel(totalScore, hasExtremeSingle);

            return (totalScore, riskLevel);
        }

        // ── Individual parameter scoring (NHS NEWS table) ──────────

        public static int ScoreRespirationRate(int respRate)
        {
            if (respRate <= 8) return 3;
            if (respRate <= 11) return 1;
            if (respRate <= 20) return 0;
            if (respRate <= 24) return 2;
            return 3; // ≥25
        }

        public static int ScoreOxygenSaturation(int spo2)
        {
            if (spo2 <= 91) return 3;
            if (spo2 <= 93) return 2;
            if (spo2 <= 95) return 1;
            return 0; // ≥96
        }

        public static int ScoreSupplementalOxygen(bool onSupplementalO2)
        {
            return onSupplementalO2 ? 2 : 0;
        }

        public static int ScoreTemperature(float temperature)
        {
            if (temperature <= 35.0f) return 3;
            if (temperature <= 36.0f) return 1;
            if (temperature <= 38.0f) return 0;
            if (temperature <= 39.0f) return 1;
            return 2; // ≥39.1
        }

        public static int ScoreSystolicBP(int systolicBP)
        {
            if (systolicBP <= 90) return 3;
            if (systolicBP <= 100) return 2;
            if (systolicBP <= 110) return 1;
            if (systolicBP <= 219) return 0;
            return 3; // ≥220
        }

        public static int ScoreHeartRate(int heartRate)
        {
            if (heartRate <= 40) return 3;
            if (heartRate <= 50) return 1;
            if (heartRate <= 90) return 0;
            if (heartRate <= 110) return 1;
            if (heartRate <= 130) return 2;
            return 3; // ≥131
        }

        public static int ScoreConsciousness(ConsciousnessLevel level)
        {
            return level == ConsciousnessLevel.Alert ? 0 : 3;
        }

        // ── Risk level determination ───────────────────────────────

        private static NewsRiskLevel DetermineRiskLevel(int totalScore, bool hasExtremeSingle)
        {
            if (totalScore >= 7)
                return NewsRiskLevel.High;

            if (totalScore >= 5 || hasExtremeSingle)
                return NewsRiskLevel.Medium;

            return NewsRiskLevel.Low;
        }
    }
}
