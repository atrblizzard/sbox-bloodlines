using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
	[Library( "item_w_uzi", Title = "UZI" )]
	public class Uzi : WeaponBase
	{
		public override string WeaponName => "UZI";
		public override int Bucket => 2;
		public override HoldType HoldType => HoldType.Pistol;
		public override string ViewModelPath => "models/weapons/submachine_uzi/view/v_submachine_uzi.vmdl";
		public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
		public override string WorldModelPath => "models/weapons/submachine_uzi/wield/w_f_submachine_uzi.vmdl";
		public override int WeaponEnum => (int)WeaponSelector.Uzi;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/uzi.png";
		public override bool DrawCrosshairLines => false;
		public override int FOV => 65;
		public override float WalkAnimationSpeedMod => 0.85f;

		public override string DeploySound => "uzi.deploy";
		public override string ReloadSound => "uzi.reload";

		public Uzi()
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

				DryFireSound = "uzi.empty",
				ShootSound = "uzi.attack_1",

				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",

				InfiniteAmmo = InfiniteAmmoType.reserve
			};

			ZoomAnimData = new AngPos
			{
				Angle = new Angles( -0.7f, -5.4f, -7f ),
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
