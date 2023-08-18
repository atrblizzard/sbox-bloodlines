namespace Vampire.System.VData.Rules.Data;

public struct VampHealInfo
{
    public float Rate { get; set; } //"1.5" //Seconds

    // See below values for what this is a percentage *OF*
    public int Percent { get; set; }

    // Note: ONLY *1* of these should be true!!
    public int PercentIsOfMaxHealth { get; set; }
    public int PercentIsOfCurrentHealth { get; set; }
    public int PercentIsOfCurrentDamage { get; set; }

    public int AggravatedHealDelayPercent { get; set; }
    public VampFeedingHealInfo VampFeedingHealInfo { get; set; }
}
