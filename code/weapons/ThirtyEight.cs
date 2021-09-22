using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
    [Library("item_w_thirtyeight", Title = "Thirty Eight")]
    public class ThirtyEight : WeaponBase
	{
		public override string WeaponName => "Thirty Eight";
        public override int Bucket => 1;
        public override HoldType HoldType => HoldType.Pistol;
        public override string ViewModelPath => "models/weapons/thirtyeight/view/v_thirtyeight.vmdl";
        public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
        public override string WorldModelPath => "models/weapons/thirtyeight/wield/w_f_thirtyeight.vmdl";
        public override int WeaponEnum => (int)WeaponSelector.ThirtyEight;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/thirtyeight.png";
        public override bool DrawCrosshairLines => false;
        public override int FOV => 65;
        public override float WalkAnimationSpeedMod => 0.85f;
        public override string DeploySound => "thirtyeight.deploy";
        public override string ReloadSound => "thirtyeight.reload";

        public ThirtyEight()
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
