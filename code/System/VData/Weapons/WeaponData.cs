using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Vampire.Data.Dialog;
using Vampire.System.VData.Weapons.Data;

namespace Vampire.System.VData.Weapons;

public enum WeaponType
{
	Firearm,
	Melee
}

public enum CameraSwitch
{
	NoSwitch,
	Melee
}

[GameResource("Weapon Data", "weapon", "VTM:B Weapon Definitions", Icon = "🔫", IconBgColor = "#ff6861", IconFgColor = "#0e0e0e" )]
public class WeaponData : GameResource
{
	public string EngineClass { get; set; }
	public string PrintName { get; set; }
	public string Description { get; set; }

    public int Weight { get; set; }
    public WeaponType ItemType { get; set; }

    public int ItemWorth { get; set; }
    public int PlayerSell { get; set; }
    
    public CameraSwitch CameraType { get; set; }
    
    [ResourceType("vmdl")]
    public string Viewmodel { get; set; }
    
    [ResourceType( "vmdl" )]
    public string WorldModel { get; set; }
    
    [ResourceType( "vmdl" )]
    public Dictionary<Gender, string> WorldModels { get; set; }

    public Magazine Magazine { get; set; }
    public List<Activation> Activations { get; set; }
    public Tables Tables { get; set; }
    public SpriteData SpriteData { get; set; }
    
    private Dictionary<string, WeaponOwnerData> ownerDictionary;
    public List<WeaponOwnerData> Owners { get; set; }

    /// <summary>
    /// Weapon data constructor.
    /// </summary>
    public WeaponData()
	{
		PrintName = "!!TITLE!!";
		Description = "!!DESCRIPTION!!";
	}

    protected override void PostLoad()
    {
	    Precache.Add( WorldModel );
	    ownerDictionary = Owners?.ToDictionary(g => g.OwnerResource);

	    // Add this asset to the registry.
	    All.Add( this );
    }

    protected override void PostReload()
    {
	    ownerDictionary = Owners?.ToDictionary( g => g.OwnerResource );
    }
    
    public WeaponBase CreateInstance()
    {
	    if ( string.IsNullOrEmpty( EngineClass ) )
		    return null;

	    var type = TypeLibrary.GetType<WeaponBase>( EngineClass ).TargetType;
	    if ( type == null )
		    return null;

	    var weapon = TypeLibrary.Create<WeaponBase>( EngineClass );
	    weapon.Initialize( this );

	    return weapon;
    }
    
    public static List<WeaponData> All { get; set; } = new();
    
    public bool CanBeOwnedByPlayerClan( PlayerClan pclass )
    {
	    return ownerDictionary?.ContainsKey( pclass.ResourcePath ) ?? false;
    }
    
    public bool TryGetOwnerDataForPlayerClan( PlayerClan pclass, out WeaponOwnerData data )
    {
	    data = default;
	    return ownerDictionary?.TryGetValue( pclass.ResourcePath, out data ) ?? false;
    }
    
    public static IEnumerable<WeaponData> FindAllForClassAndSlot( PlayerClan pclass, WeaponSlot slot )
    {
	    return All.Where( x => x.TryGetOwnerDataForPlayerClan( pclass, out var ownerData ) && ownerData.Slot == slot );
    }
}
