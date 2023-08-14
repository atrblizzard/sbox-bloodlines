using Sandbox;

namespace Vampire.System.VData.Rules.Data;

/// <summary>
/// Frenzy Checks occur:
///     a) If you take more than the below damage in one hit
///     b) If you take more than the below Aggravated damage in one hit
///     c) Via Dialog-forced checks
///     d) Hunger checks
///     e) Scripted triggers
/// </summary>
public struct VampFrenzyInfo
{
    /// <summary>
    /// Amount of damage in one hit to trigger a FrenzyCheck
    /// </summary>
    public int DamageAmount { get; set; }

    /// <summary>
    /// Amount of Aggravated damage in one hit to trigger a FrenzyCheck
    /// </summary>
    /// 
    public int AggravatedDamageAmount { get; set; }

    /// <summary>
    /// Delay for next Frenzy check because you just Frenzied!  // 3 Minutes plus 10 second frenzy time
    /// </summary>
    public float SuccessCoolDownTime { get; set; }

    /// <summary>
    /// Frenzy failed to occur, delay for next time to check
    /// </summary>
    public float FailureCoolDownTime { get; set; }

    /// <summary>
    /// "5" Default Difficulty for Frenzy Checks
    /// </summary>
    public int DefaultDifficulty { get; set; }

    /// <summary>
    /// If you have less than this blood, you get a penalty on your Humanity check for Frenzy
    /// </summary>
    public int BloodPoolMinForHunger { get; set; }

    /// <summary>
    /// If you have less than this blood, you get a penalty on your Humanity check for Frenzy
    /// </summary>
    public int BloodPoolMinPenalty { get; set; }

    /// <summary>
    /// Difficulty level for Hunger Checks
    /// </summary>
    public int HungerDifficulty { get; set; }
}