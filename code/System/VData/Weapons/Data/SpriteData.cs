using Sandbox;
using Sandbox.UI;

namespace Vampire.System.VData.Weapons.Data;

public struct SpriteData
{
    [ResourceType( "png" )] public string Weapon { get; set; }
    [ResourceType( "png" )] public string WeaponSelected { get; set; }
    [ResourceType( "png" )] public string Ammo { get; set; }
    [ResourceType( "png" )] public string AmmoSelected { get; set; }
}
