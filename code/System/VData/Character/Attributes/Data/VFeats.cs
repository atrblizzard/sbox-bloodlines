namespace Vampire.System.VData.Character.Attributes.Data;

public struct VFeats
{
    //Combat:
    public int Brawl { get; set; } // Unarmed
    public int Melee { get; set; }
    public int RangedCombat { get; set; }
    public int Defense { get; set; }

    //Physical / Covert:
    public int Intrusion { get; set; }
    public int Jumping { get; set; }
    public int Sneaking { get; set; }
    public int Throwing { get; set; }

    //Mental:
    public int AnimalFriendship { get; set; }
    //public int Intimidate { get; set; }
    public int Hacking { get; set; }
    public int Inspection { get; set; }
    public int Research { get; set; }

    // Mental / Social
    public int Haggle { get; set; }
    public int Intimidate { get; set; }
    public int Persuation { get; set; }
    public int Seduction { get; set; }
}