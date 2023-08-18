using Sandbox;
using System.Collections.Generic;
using Vampire.System.VData.Weapons.Data;

namespace Vampire.System.VData.Weapons;

[GameResource("Weapon Data", "weapon", "VTM:B Weapon Definitions")]
public class WeaponData : GameResource
{
	public string InternalName { get; set; }
	public string PrintName { get; set; }
	public string Description { get; set; }

    public int Weight { get; set; }
    public string ItemType { get; set; }

    public int ItemWorth { get; set; }
    public int PlayerSell { get; set; }

    public Magazine Magazine { get; set; }
    public List<Activation> Activations { get; set; }
    public Tables Tables { get; set; }
    public SpriteData SpriteData { get; set; }

    /// <summary>
    /// Weapon data constructor.
    /// </summary>
    public WeaponData()
	{
		PrintName = "!!TITLE!!";
		Description = "!!DESCRIPTION!!";
	}
}
