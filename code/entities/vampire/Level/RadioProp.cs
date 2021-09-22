using Sandbox;

[Library( "prop_radio", Description = "Radio" )]
[Hammer.Model]
public partial class RadioProp : ModelEntity, IUse
{
	/// <summary>
	/// The volume of playing sound.
	/// </summary>
	[Property( "volume", Title = "The volume of playing sound." )]
	public float Volume { get; set; } = 80f;

	/// <summary>
	/// Maximum radio hearing radius, in units.
	/// </summary>
	[Property( "radius", Title = "Maximum radio hearing radius, in units." )]
	public float Radius { get; set; } = 800f;

	[Input]
	public void Activate()
	{

	}

	[Input]
	public void Deactivate()
	{

	}

	[Input]
	public void SetSoundOverrideEnt()
	{

	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		return false;
	}

	public void Use()
	{
		
	}

	public void LoadRadioData()
	{

	}

	public override void Spawn()
	{
		base.Spawn();

		SetModel( GetModelName() );

		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}
}
