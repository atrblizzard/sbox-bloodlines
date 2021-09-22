using Sandbox;
using SWB_Base;

namespace Bloodlines.Weapons
{
    [Library("item_w_deserteagle", Title = "Desert Eagle")]
    public class DesertEagle : WeaponBase
    {
		public override string WeaponName => "IMI Desert Eagle";
		public override int Bucket => 1;
        public override HoldType HoldType => HoldType.Pistol;
        public override string ViewModelPath => "models/weapons/desert_eagle/view/v_desert_eagle.vmdl";
        public override string ViewModelHandPath => "models/hands/female/shared/v_shared_female_hands.vmdl";
        public override string WorldModelPath => "models/weapons/desert_eagle/wield/w_m_deserteagle.vmdl";
        public override int WeaponEnum => (int)WeaponSelector.DesertEagle;
		public override string Icon => "ui/hud/inventory_images/weapons_ranged/deserteagle.png";
		public override bool DrawCrosshairLines => false;
        public override int FOV => 65;
        public override float WalkAnimationSpeedMod => 0.85f;
        public override string DeploySound => "desert_eagle.deploy";
        public override string ReloadSound => "desert_eagle.reload";

        public DesertEagle()
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
                ShootSound = "desert_eagle.attack_1",

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
