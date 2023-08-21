using Sandbox;
using Amper.FPS;

namespace Vampire;

public abstract partial class MapGamemode : Entity, IGamemode
{
	public virtual string Title => ClassName;
	public virtual string Icon => IGamemode.DEFAULT_ICON;
	public virtual GamemodeProperties Properties => default;
	public int Priority => 0;

	public MapGamemode()
	{
		EventDispatcher.Subscribe<RoundEndEvent>( RoundEnd, this );
		EventDispatcher.Subscribe<RoundActiveEvent>( RoundActivate, this );
		EventDispatcher.Subscribe<RoundRestartEvent>( RoundRestart, this );
	}

	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;
	}

	public virtual void Reset() { }
	public virtual bool IsActive() => true;
	public abstract bool HasWon( out Team team, out WinReason reason );

	[GameEvent.Tick.Server] public virtual void Tick() { }
	[GameEvent.Entity.PostSpawn] public virtual void PostLevelSetup() { }

	public virtual void RoundEnd( RoundEndEvent args ) { }
	public virtual void RoundActivate( RoundActiveEvent args ) { }
	public virtual void RoundRestart( RoundRestartEvent args ) { Reset(); }
    public virtual bool ShouldSwapTeams(Team winner, WinReason winReason)
    {
		return Properties.SwapTeamsOnRoundRestart;
    }
}
