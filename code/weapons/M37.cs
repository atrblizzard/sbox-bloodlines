using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
	[Library( "item_w_itacha_m_37" )]
	public class M37 : WeaponBase
	{
		public override string WeaponName => "Utica M37";
		public override int Bucket => 3;
		public override HoldType HoldType => HoldType.Shotgun;
		public override string ViewModelPath => "models/weapons/m37/view/v_m37.vmdl";
		public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
		public override string WorldModelPath => "models/weapons/m37/wield/w_f_v_m37.vmdl";
		public override int WeaponEnum => (int)WeaponSelector.M37;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/shotgun.png";
		public override bool DrawCrosshairLines => false;
		public override int FOV => 45;
		public override float WalkAnimationSpeedMod => 0.85f;

		public override string DeploySound => "shotgun.deploy";
		public override string ReloadSound => "shotgun.reload";

		public M37()
		{
			Primary = new ClipInfo
			{
				Ammo = 8,
				AmmoType = AmmoType.Shotgun,
				ClipSize = 8,

				Bullets = 8,
				BulletSize = 2f,
				Damage = 15f,
				Force = 5f,
				Spread = 0.3f,
				Recoil = 2f,
				RPM = 80,
				FiringType = FiringType.semi,
				ScreenShake = new ScreenShake
				{
					Length = 0.5f,
					Speed = 4.0f,
					Size = 1.0f,
					Rotation = 0.5f
				},

				DryFireSound = "shotgun.empty",
				ShootSound = "shotgun.attack_1",

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

