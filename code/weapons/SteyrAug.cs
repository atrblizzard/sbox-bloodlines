using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
	[Library( "item_w_steyr_aug", Title = "Steyr Aug" )]
	public class SteyrAug : WeaponBase
	{
		public override string WeaponName => "Steyr Aug";
		public override int Bucket => 4;
		public override HoldType HoldType => HoldType.Rifle;
		public override string ViewModelPath => "models/weapons/rifle_steyraug/view/v_rifle_steyraug.vmdl";
		public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
		public override string WorldModelPath => "models/weapons/rifle_steyraug/wield/w_f_rifle_steyraug.vmdl";
		public override int WeaponEnum => (int)WeaponSelector.SteyrAug;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/steyr_aug.png";
		public override bool DrawCrosshairLines => false;
		public override int FOV => 65;
		public override float WalkAnimationSpeedMod => 0.85f;

		public override string DeploySound => "steyr_aug.deploy";
		public override string ReloadSound => "steyr_aug.reload";

		// Animations
		public override string IdleSequence => "idle01";
		public override string AttackSequence => "fire01";
		public override string ReloadSequence => "reload";

		public SteyrAug()
		{
			Primary = new ClipInfo
			{
				Ammo = 32,
				AmmoType = AmmoType.SMG,
				ClipSize = 32,
				ReloadTime = 3.14f,

				BulletSize = 2f,
				Damage = 12f,
				Force = 3f,
				Spread = 0.2f,
				Recoil = 0.9f,
				RPM = 1090,
				FiringType = FiringType.auto,
				ScreenShake = new ScreenShake
				{
					Length = 0.5f,
					Speed = 4.0f,
					Size = 0.5f,
					Rotation = 0.5f
				},

				DryFireSound = "steyr_aug.empty",
				ShootSound = "steyr_aug.attack_1",

				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",

				InfiniteAmmo = InfiniteAmmoType.reserve
			};

			ZoomAnimData = new AngPos
			{
				Angle = new Angles( 0f, 0.5f, -2f ),
				Pos = new Vector3( -5.5f, 4f, -2f )
			};

			RunAnimData = new AngPos
			{
				Angle = new Angles( 10, 40, 0 ),
				Pos = new Vector3( 5, 0, 0 )
			};
		}
	}
}
