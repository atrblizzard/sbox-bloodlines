using Bloodlines.Weapons;
using Sandbox;
using SWB_Base;

partial class VampirePlayer : PlayerBase
{
    public VampirePlayer() : base() { }

    public override void Respawn()
    {
        base.Respawn();

        SupressPickupNotices = true;

		SetModel( "models/character/pc/female/tremere/armor0/tremere_female_armor_0.vmdl" );

#if VR_ENABLED
		if ( VR.Enabled )
			Inventory.Add( new VR_Hands(), VR.Enabled );
#endif

#if SANDBOX_ENABLED
		Inventory.Add( new PhysGun(), !VR.Enabled );
		Inventory.Add( new GravGun() );
		Inventory.Add( new Tool() );
		Inventory.Add( new Pistol() );
		Inventory.Add( new Flashlight() );
#endif

		Inventory.Add(new Glock());
        Inventory.Add(new Anaconda());
        Inventory.Add(new DesertEagle());
        Inventory.Add(new ThirtyEight());
        Inventory.Add(new Mac10());

		Inventory.Add( new Uzi() );

		Inventory.Add( new M37() );
		Inventory.Add( new SuperShotgun() );

		Inventory.Add( new SteyrAug() );
		Inventory.Add( new Rem700() );

		Inventory.Add( new Crossbow() );

		GiveAmmo(AmmoType.SMG, 100);
        GiveAmmo(AmmoType.Pistol, 60);
        GiveAmmo(AmmoType.Revolver, 60);
        GiveAmmo(AmmoType.Rifle, 60);
        GiveAmmo(AmmoType.Shotgun, 60);
        GiveAmmo(AmmoType.Sniper, 60);
        GiveAmmo(AmmoType.Grenade, 60);
        GiveAmmo(AmmoType.RPG, 60);

#if ENABLE_ATTRIBUTES
		InitializeAttributes();
#endif

		Toast( "You just got spawned!" );

		SupressPickupNotices = false;
    }	

	public void Toast( string message, bool localize = true )
	{
		if ( string.IsNullOrWhiteSpace( message ) ) return;
		using ( Prediction.Off() )
		{
			OnClientToast( To.Single( this ), message, localize );
		}
	}

	[ClientRpc]
	protected void OnClientToast( string message, bool localize )
	{
		if ( DeathmatchHud.Current is not DeathmatchHud hud ) return;

		hud.Toast( message );
	}
}
