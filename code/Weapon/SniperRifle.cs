using Sandbox;

namespace Vampire;

public partial class SniperRifle : WeaponBase
{
	public const float ScopedMaxSpeed = 80;
	public virtual float ZoomLevelCooldown => 0.3f;
	public virtual float ZoomedFieldOfView => 30;
	public virtual float ChargeMultiplier => 3;
	public virtual int MaxZoomLevel => 1;
	public bool IsZoomed => ZoomLevel > 0;
	[Net, Predicted] public int ZoomLevel { get; set; }
	public bool CanChangeZoomLevel => TimeSinceChangedZoomLevel >= ZoomLevelCooldown;
	[Net, Predicted] TimeSince TimeSinceChangedZoomLevel { get; set; }
	float? AutoZoomOutTime { get; set; }
	bool WillAutoZoomIn { get; set; }
	
	
	public bool CanZoomIn()
	{
		// Only allow zoom in if we can attack.
		if ( !CanPrimaryAttack() )
			return false;

		if ( !HasEnoughAmmoToAttack() )
			return false;

		return true;
	}

	public bool CanZoomOut()
	{
		return true;
	}
	
	public bool ChargeBellPlayed { get; set; }
	public float Charge { get; set; }
	
	protected override void DebugScreenText( float interval )
	{
		DebugOverlay.ScreenText(
			$"[SNIPER RIFLE]\n" +
			$"MaxZoomLevel          {MaxZoomLevel}\n" +
			$"ZoomLevel             {ZoomLevel}\n" +
			$"IsZoomed              {IsZoomed}\n" +
			$"CanChangeZoomLevel    {CanChangeZoomLevel}\n" +
			$"WillAutoZoomIn        {WillAutoZoomIn}\n" +
			$"AutoZoomOutTime       {AutoZoomOutTime}\n" +
			$"Charge                {Charge}\n" +
			$"ChargeBellPlayed      {ChargeBellPlayed}\n" +
			$"\n" +
			$"CanZoomIn()           {CanZoomIn()}\n" +
			$"CanZoomOut()          {CanZoomOut()}\n",
			interval
		);
	}
}