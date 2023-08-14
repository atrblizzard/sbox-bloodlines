namespace Vampire.System.VData.Rules.Data;

public struct VampFeedingHealInfo
{
    public bool UsesRatio { get; set; }
    public int BloodToHealthRatio { get; set; } // So 1 blood point == 10 Health
}