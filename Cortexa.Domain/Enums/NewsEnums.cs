namespace Cortexa.Domain.Enums
{
    /// <summary>
    /// AVPU consciousness level used in NEWS scoring.
    /// </summary>
    public enum ConsciousnessLevel
    {
        Alert,
        Voice,
        Pain,
        Unresponsive
    }

    /// <summary>
    /// NEWS clinical risk level based on total score.
    /// </summary>
    public enum NewsRiskLevel
    {
        Low,
        Medium,
        High
    }
}
