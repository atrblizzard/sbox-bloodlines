namespace Vampire;

public partial class WeaponBase
{
	public override int GetAmmoPerShot() => 1; //Data.AmmoPerShot;
	public override int GetBulletsPerShot() => 1; //Data.BulletsPerShot;
	public override float GetDamage() => 30; //Data.Damage;
	public override int GetTracerFrequency() => 2; //Data.TracerFrequency;
	public override int GetRange() => 4096; //Data.Range;
	public override float GetSpread() => 0.04f; //Data.BulletSpread;
	public override int GetClipSize() => 12; // Data.ClipSize;
	public override int GetReserveSize() => 32;
	public override bool IsReloadingEntireClip() => true; //Data.ReloadsEntireClip;

	public override float GetAttackTime() => 0.15f; //Data.AttackTime;
	public override float GetReloadStartTime() => 0;// Data.ReloadStartTime;
	public override float GetReloadTime() => 1; //Data.ReloadTime;
	public override float GetDeployTime() => 0.5f; //Data.DeployTime;
}