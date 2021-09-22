using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
    /// <summary>
    /// Glock 17c
    /// </summary>
    [Library("item_w_glock17c", Title = "Glock")]
    public class Glock : WeaponBase
    {
		public override string WeaponName => "GLOCK 19";
		public override int Bucket => 1;
        public override HoldType HoldType => HoldType.Pistol;
        public override string ViewModelPath => "models/weapons/pistol_glock/view/v_pistol_glock.vmdl";
        public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
        public override string WorldModelPath => "models/weapons/pistol_glock/wield/w_f_pistol_glock.vmdl";
        public override int WeaponEnum => (int)WeaponSelector.Glock;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/glock.png";
        public override bool DrawCrosshairLines => false;
        public override int FOV => 55;
        public override int ZoomFOV => 60;
        public override string ReloadSound => "glock.reload";
        public override string DeploySound => "glock.deploy";

		//public override float TuckRange => -1;        

		// Animations
		public override string IdleSequence => "idle01";
		public override string AttackSequence => "fire01";
		public override string ReloadSequence => "reload01";
		public override string DrawSequence => "draw01";
		public override string LowerSequence => "lower01";

		public Glock()
        {
            Primary = new ClipInfo
            {
                Ammo = 18,
                AmmoType = AmmoType.Pistol,
                ClipSize = 18,
                ReloadTime = 2.17f,

                BulletSize = 6f,
                Damage = 50f,
                Force = 5f,
                Spread = 0.06f,
                Recoil = 1f,
                RPM = 300,
                FiringType = FiringType.semi,
                ScreenShake = new ScreenShake
                {
                    Length = 0.5f,
                    Speed = 4.0f,
                    Size = 1.0f,
                    Rotation = 0.5f
                },

                DryFireSound = "swb_pistol.empty",
                ShootSound = "glock.attack_1",
                //ReloadSound = "glock.reload",

                BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
                MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",

                InfiniteAmmo = InfiniteAmmoType.normal
            };

            ZoomAnimData = new AngPos
            {
                Angle = new Angles(0, -0.1f, 0),
                Pos = new Vector3(-5.125f, 0, 2.67f)
            };

            RunAnimData = new AngPos
            {
                Angle = new Angles(0, 1, 0),
                Pos = new Vector3(0, 0, 0)
            };
        }
    }
}
