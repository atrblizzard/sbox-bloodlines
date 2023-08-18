namespace Vampire.System.VData.Weapons.Data;

public struct Activation
{
    public string Tag { get; set; }
    public string Type { get; set; }
    public string AmmoType { get; set; }
    public int AmmoCost { get; set; }

    public int SkillRequirement { get; set; } // min. skill requirement used in dmg calculations
    public int BaseLethality { get; set; }
    public string Damage { get; set; }

    public float AttackRate { get; set; }
    public string BotchTable { get; set; }
    public int Range { get; set; }

    public float Accuracy { get; set; }
    public float SpreadAngle { get; set; }
    public int CriminalLevel { get; set; }

    public SoundData SoundData { get; set; }
}