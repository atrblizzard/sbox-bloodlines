using Sandbox;

namespace Vampire;

public class WeaponOwnerData
{
	[ResourceType( "vclan" ), Title("Owning Class")]
	public string OwnerResource { get; set; }
	/// <summary>
	/// Slot to assign this weapon to.
	/// </summary>
	public WeaponSlot Slot { get; set; }
	/// <summary>
	/// Hold Pose for Animgraph.
	/// </summary>
	public HoldPose HoldPose { get; set; }
	/// <summary>
	/// Maximum carried ammo for this weapon when class wears this weapon.
	/// </summary>
	public int Reserve { get; set; }
	/// <summary>
	/// If true, will use "c_" model system
	/// </summary>
	public bool AttachToHands { get; set; }
	/// <summary>
	/// This is the view model of the weapon.<br/>
	/// <b>It will only be used for if "View Model Mode" is set to either "Parent Hands To Weapon" or "Weapon Only".</b>
	/// </summary>
	[ResourceType( "vmdl" )]
	public string ViewModel { get; set; }
}