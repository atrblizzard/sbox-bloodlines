using Editor;
using Sandbox;

namespace Bloodlines.Entities.core.Logic;

[ClassName( "logic_timer" )]
[HammerEntity]
[VisGroup( VisGroup.Logic )]
[EditorSprite("materials/editor/logic_timer.vmat")]
[Title( "logic_timer" ), Category( "Logic" ), Icon( "timer" )]
public partial class LogicTimer : Entity
{
	/// <summary>
	/// The (initial) enabled state of the logic entity.
	/// </summary>
	[Property]
	public bool Enabled { get; set; } = true;
	
	[Property]
	public float InitialDelay { get; set; } = 0;
	
	/// <summary>
	/// If the time should be randomly picked. If not checked, the timer will always be MaxTime long.
	/// </summary>
	public bool RandomizeTime { get; set; } = false;
	public float MaxTime { get; set; } = 10f;
	public float MinTime { get; set; }

	/// <summary>
	/// Enables the entity.
	/// </summary>
	[Input]
	public void Enable()
	{
		Enabled = true;
	}

	/// <summary>
	/// Disables the entity, so that it would not fire any outputs.
	/// </summary>
	[Input]
	public void Disable()
	{
		Enabled = false;
	}
	/// <summary>
	/// Enables the entity.
	/// </summary>
	[Input]
	public void TurnOn()
	{
		Enabled = true;
	}

	/// <summary>
	/// Disables the entity, so that it would not fire any outputs.
	/// </summary>
	[Input]
	public void TurnOff()
	{
		Enabled = false;
	}
	/// <summary>
	/// Toggles the enabled state of the entity.
	/// </summary>
	[Input]
	public void Toggle()
	{
		Enabled = !Enabled;
	}

	[Input]
	public void Kill()
	{
		if(Sandbox.Game.IsServer) Delete();
	}


	/// <summary>
	/// Fired when the this entity receives the "Trigger" input.
	/// </summary>
	protected Output OnTrigger { get; set; }
	
	protected Output OnTimerLow { get; set; }
	
	protected Output OnTimerHigh { get; set; }

	/// <summary>
	/// Trigger the "OnTrigger" output.
	/// </summary>
	[Input]
	public void Trigger()
	{
		if ( !Enabled ) return;

		OnTrigger.Fire( this );
	}
	
	public override void Spawn()
	{
		base.Spawn();

		PickTime();
	}

	TimeSince timeSinceStart;
	bool paused;
	float timePassed;
	float timeToPass;
	[Event.Tick.Server]
	private void Tick()
	{
		if(Enabled)
		{
			if(paused)
			{
				Unpause();
			}
			if(timeSinceStart >= timeToPass)
			{
				Finish();
			}
		}
		else
		{
			if(!paused)
			{
				Pause();
			}
		}
	}

	private void PickTime()
	{
		timeToPass = RandomizeTime ? Sandbox.Game.Random.Float( MinTime, MaxTime ).Clamp(0, MaxTime) : MaxTime;
	}

	private void Finish()
	{
		OnTrigger.Fire( null );

		PickTime();
	}

	private void Unpause()
	{
		paused = false;
		timeSinceStart = -timePassed;
	}

	private void Pause()
	{
		paused = true;
		timePassed = timeSinceStart;
	}
}
