using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
    [Library("item_w_colt_anaconda", Title = "Anaconda")]
    public class Anaconda : WeaponBase
    {
		public override string WeaponName => "Colt Anaconda";
		public override int Bucket => 1;
        public override HoldType HoldType => HoldType.Pistol;
        public override string ViewModelPath => "models/weapons/anaconda/view/v_anaconda.vmdl";
        public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
        public override string WorldModelPath => "models/weapons/anaconda/wield/w_f_anaconda.vmdl";
        public override int WeaponEnum => (int)WeaponSelector.Anaconda;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/anaconda.png";
		public override bool DrawCrosshairLines => false;
        public override int FOV => 65;
        public override float WalkAnimationSpeedMod => 0.85f;
        public override string DeploySound => "anaconda.deploy";
        public override string ReloadSound => "anaconda.reload";

        public Anaconda()
        {
            Primary = new ClipInfo
            {
                Ammo = 7,
                AmmoType = AmmoType.Revolver,
                ClipSize = 7,
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
                ShootSound = "anaconda.attack_1",

                BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
                MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",

                InfiniteAmmo = InfiniteAmmoType.reserve
            };

            ZoomAnimData = new AngPos
            {
                Angle = new Angles(0, -0.1f, 0),
                Pos = new Vector3(-5.125f, 0, 2.67f)
            };

            RunAnimData = new AngPos
            {
                Angle = new Angles(-40, 0, 0),
                Pos = new Vector3(0, -3, -8)
            };
        }
    }
}
