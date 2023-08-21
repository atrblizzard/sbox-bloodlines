using Amper.FPS;
using Sandbox;
using Vampire.System.VData.Weapons;
using Vampire.System.VData.Weapons.Data;
using NotImplementedException = System.NotImplementedException;

namespace Vampire;

public interface IFalloffProvider
{
	public bool UseFalloff { get; }
	public bool UseRampup { get; }
}


public abstract partial class WeaponBase : SDKWeapon, IUse, IFalloffProvider
{
	[Net] public int MaxReserve { get; set; }
	[Net] public bool AttachToHands { get; set; }
	public Team Team => (Team)TeamNumber;
	[Net] public HoldPose HoldPose { get; set; }
	
	public bool IsInitialized => Data != null;
	[Net] public WeaponData Data { get; set; }
	
	public WeaponSlot Slot => (WeaponSlot)SlotNumber;

	public WeaponAnimSlot AnimSlot { get; set; }
	
	public VampirePlayer VampireOwner => Owner as VampirePlayer;

	bool IFalloffProvider.UseFalloff => true; //Data.UseFalloff;

	bool IFalloffProvider.UseRampup => true; //Data.UseRampup;
	
	public void Initialize( WeaponData data )
	{
		Data = data;

		SetModel( Data.WorldModel );
		Clip = Data.Magazine.DefaultSize;
		EnableShadowInFirstPerson = true;
		EnableShadowCasting = true;
	}
	
	public bool OnUse(Entity user)
	{
		if ( user is VampirePlayer player && Game.IsServer )
			player.EquipWeapon( this, true );

		return false;
	}

	public bool IsUsable(Entity user)
	{
		// Can't be used if we already have an owner.
		if ( Owner.IsValid() )
			return false;
		
		// Can't be used if it runs out of ammo.
		if ( Clip <= 0 && Reserve <= 0 && NeedsAmmo() )
			return false;

		if ( user is VampirePlayer player )
		{
			var pclan = player.PlayerClan;
			return Data.IsValid() && Data.CanBeOwnedByPlayerClan(pclan);
		}

		return false;
	}
	
	public override void OnEquip( SDKPlayer owner )
	{
		base.OnEquip( owner );
		DroppedAutoDestroyTime = null;

		if ( owner is not VampirePlayer player )
			return;

		if ( !Data.TryGetOwnerDataForPlayerClan( player.PlayerClan, out var ownerData ) )
			return;

		OnEquippedByNewOwner(player, ownerData);
	}

	public void OnEquippedByNewOwner( VampirePlayer player, WeaponOwnerData ownerData)
	{
		// Put the item in the correct slot.
		SlotNumber = 1;
		MaxReserve = 32;
		AttachToHands = true;

		// SlotNumber = (int)ownerData.Slot;
		// MaxReserve = ownerData.Reserve;
		// HoldPose = ownerData.HoldPose;
		// AttachToHands = ownerData.AttachToHands;
	}
	
	public float? DroppedAutoDestroyTime { get; set; }

	public override void OnDrop( SDKPlayer owner )
	{
		base.OnDrop( owner );
		DroppedAutoDestroyTime = Time.Now + v_dropped_weapon_lifetime;
	}
	
	[ConVar.Server] public static float v_dropped_weapon_lifetime { get; set; } = 30;
	[ConVar.Replicated] public static bool v_use_fixed_weaponspreads { get; set; } = false;
	
	public override string GetViewModelPath()
	{
		if (Data.TryGetOwnerDataForPlayerClan(VampireOwner.PlayerClan, out var ownerData))
		{
			if (!string.IsNullOrEmpty(ownerData.ViewModel))
			{
				return ownerData.ViewModel;
			}
		}

		return Data.Viewmodel;
	}

	public override void SetupAnimParameters()
	{
		base.SetupAnimParameters();
		SendPlayerAnimParameter( "weapon_slot", (int)HoldPose );
	}

	public override void SendAnimParametersOnAttack()
	{
		var vm = (ViewModel)GetViewModelEntity();
		vm?.GetAttachment().SetAnimParameter("b_fire", false);

		//Log.Info("Firing shit here!");
		SendAnimParameter( "b_fire" );
	}
	
	public override void SendAnimParametersOnReloadStart()
	{
		SendAnimParameter( "b_reload", true );
	}
	public override void SendAnimParametersOnReloadStop()
	{
		SendAnimParameter( "b_reload", false );
	}
	
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( sv_debug_weapons && IsLocalPawn )
		{
			DebugScreenText( new Vector2( 60, 450 ), 0.5f );
		}
	}
	
	public override void PlayAttackSound()
	{
		PlaySound( Data.Activations[0].SoundData.SoundEntry[SoundType.Attack] );
	}
	
	public void PlayReloadSound()
	{
		PlaySound( Data.Activations[0].SoundData.SoundEntry[SoundType.Reload] );
	}

	protected void DebugScreenText(Vector2 position, float interval)
	{
		DebugOverlay.ScreenText(
			$"[WeaponBase]\n" +
			$"Name                  {Name}\n" +
			$"Model Name            {GetModelName()}\n" +
			$"ViewModel:            {GetViewModelPath()}\n" +
			$"Active Weapon:        {VampireOwner?.ActiveWeapon}\n" +
			$"Owner:                {Owner}\n" +
			$"Clip:                 {Clip}\n" +
			$"Reserve:              {Reserve}\n" +
			$"Max Reserve:          {MaxReserve}\n" +
			$"Slot:                 {Slot}\n" +
			$"Tags:                 {string.Join(",", Tags.List)}\n" +
			$"Fire:                 {GetAnimParameterBool("b_fire")}\n" +
			$"Has AnimGraph:        {HasAnimGraph()}\n" +
			$"\n",
			position, interval);
	}
}

public enum WeaponSlot
{
	Primary = 0,
	Secondary,
	Melee,
	Misc1,
	Misc2,
	Action
}

public enum WeaponAnimSlot
{
	Glock,
	Anaconda,
	M37,

	// Don't change the order of the items in this list, 
	// otherwise it will break animgraphs. Always add new elements in the end.
}


/// <summary>
/// Weapon hold poses for animgraph.
/// </summary>
public enum HoldPose
{
	Primary,
	Secondary,
	Melee,
	Misc1,
	Misc2,
	Action,
	AllClass,
	Item1,
	Item2,
	Item3,
	Item4,
	Item5,
	Item6,
	Item7,
	Item8,
	Item9,

	// Don't change the order of the items in this list, 
	// otherwise it will break animgraphs. Always add new elements in the end.
}
