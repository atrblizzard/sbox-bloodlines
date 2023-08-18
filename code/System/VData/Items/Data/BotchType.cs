namespace Vampire.System.VData.Items.Data;

public struct BotchType
{
    public string Name { get; set; }
    public string InternalName { get; set; }

    public string OwnerDamage { get; set; }
    public bool AmmoLost { get; set; }
    public bool ItemBreaks { get; set; }
    public bool ForceReload { get; set; }
    public bool FallToGround { get; set; }

    public string ClientEffect { get; set; }
}
