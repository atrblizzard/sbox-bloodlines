using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
	[Library( "item_w_crossbow", Title = "Crossbow" )]
	public class Crossbow : WeaponBase
	{
		public override string WeaponName => "Remington M700";
		public override int Bucket => 4;
		public override HoldType HoldType => HoldType.Rifle;
		public override string ViewModelPath => "models/weapons/crossbow/view/v_crossbow.vmdl";
		public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
		public override string WorldModelPath => "models/weapons/rifle_rem700/wield/w_f_rifle_rem700.vmdl";
		public override int WeaponEnum => (int)WeaponSelector.Rem700;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/remington_m-700.png";
		public override bool DrawCrosshairLines => false;
		public override int FOV => 65;
		public override float WalkAnimationSpeedMod => 0.85f;

		public override string DeploySound => "crossbow.deploy";
		public override string ReloadSound => "crossbow.reload";

		// Animations
		public override string IdleSequence => "idle01";
		public override string AttackSequence => "fire01";
		public override string ReloadSequence => "reload";
		public override string DrawSequence => "draw";
		public override string LowerSequence => "lower";

		public Crossbow()
		{
			Primary = new ClipInfo
			{
				Ammo = 32,
				AmmoType = AmmoType.Rifle,
				ClipSize = 32,
				ReloadTime = 3.14f,

				BulletSize = 2f,
				Damage = 12f,
				Force = 3f,
				Spread = 0.2f,
				Recoil = 0.9f,
				RPM = 1090,
				FiringType = FiringType.semi,
				ScreenShake = new ScreenShake
				{
					Length = 0.5f,
					Speed = 4.0f,
					Size = 0.5f,
					Rotation = 0.5f
				},

				DryFireSound = "remington.empty",
				ShootSound = "remington.attack_1",

				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",

				InfiniteAmmo = InfiniteAmmoType.reserve
			};

			ZoomAnimData = new AngPos
			{
				Angle = new Angles( -0.7f, 0f, -4f ), //new Angles( -0.7f, -5.4f, -7f ),
				Pos = new Vector3( -6.94f, 0f, 2.9f )
			};

			RunAnimData = new AngPos
			{
				Angle = new Angles( 10, 40, 0 ),
				Pos = new Vector3( 5, 0, 0 )
			};
		}
	}
}

