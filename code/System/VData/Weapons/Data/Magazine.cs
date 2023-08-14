namespace Vampire.System.VData.Weapons.Data;

public struct Magazine
{
    public string AmmoPrintName { get; set; }
    public string AmmoDescription { get; set; }

    public int AmmoWorth { get; set; }

    public string Type { get; set; }
    public float Size { get; set; }
    public int DefaultSize { get; set; }
    public float DroppedAmmo { get; set; }
    public float ReloadTime { get; set; }
}