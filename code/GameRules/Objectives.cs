using System;
using System.Linq;
using Sandbox;

namespace Vampire;

public partial class VampireGameRules
{
	public IGamemode GetGamemode()
	{
		if(EntityGamemode != null)
		{
			return EntityGamemode;
		}

		return ClassGamemode;
	}
	[Net] private MapGamemode EntityGamemode { get; set; }
	[Net] private UniversalGamemode ClassGamemode { get; set; }
	
	public bool HasGamemode() => GetGamemode() != default;
	public bool IsPlaying<T>() where T : IGamemode => GetGamemode()?.GetType() == typeof(T); // Instead of is T to avoid subclasses triggering this, might want to reconsider this later
	public bool TryGetGamemode<T>(out T instance) where T : IGamemode
	{
		if( GetGamemode() is T mode)
		{
			instance = mode;
			return true;
		}

		instance = default;
		return false;
	}

	public override void ResetObjectives()
	{
		// reset all resettable ents
		//foreach ( var ent in All.OfType<IResettable>() ) ent.Reset();

		// Reset default ents manually
		foreach(var door in All.OfType<DoorEntity>())
		{
			door.Locked = door.SpawnSettings.HasFlag( DoorEntity.Flags.StartLocked );

			if ( door.InitialPosition > 0 )
			{
				door.FireInput("SetPosition", door, door.InitialPosition / 100.0f );
			}
		}

#if FINISHED
		// Reset respawn wave times
		RespawnWaveTimes.Clear();

		//Swap player teams if this is round end, we have a winner + the gamemode allows it
		try
		{
            if (IsRoundEnded && Winner != 0 && (
				GetGamemode().ShouldSwapTeams((Team)Winner, (WinReason)WinReason) 
				|| GameRulesRelay.Instance?.SwitchTeams == true
			))
            {
				SwapAllPlayersTeam();
            }
        }
        catch(InvalidCastException e)
		{
			Log.Error($"Failed to swap teams due to cast error: {e.Message}");
		}
#endif
    }

    public override void CalculateObjectives()
	{
		// This function helps define what game type are we currently playing.

		FindGamemode();

		if( GetGamemode() == default )
		{
			Log.Info( "No gamemode found for this map, running without gamemode..." );
			return;
		}

		Log.Info( $"We're playing: {GetGamemode().Title}" );
	}

	/// <summary>
	/// Checks all gamemodes if they would be playable on the current map.
	/// </summary>
	/// <returns></returns>
	public virtual void FindGamemode()
	{
		// Check entities first
		foreach ( var mode in Entity.All.OfType<MapGamemode>().OrderByDescending(e => e.Priority) )
		{
			if ( mode.IsActive() )
			{
				EntityGamemode = mode;
				return;
			}
		}

		// Check non-entity gamemodes after
		var gamemodes = TypeLibrary.GetTypes<UniversalGamemode>().Where(g => !g.IsAbstract);

		// Skip gamemodes which require entities to spawn
		foreach ( var mode in gamemodes.Select( g => 
			         TypeLibrary.Create<UniversalGamemode>( g.TargetType ) ).OrderByDescending( n => n.Priority) )
		{
			if ( mode.IsActive() )
			{
				ClassGamemode = mode;
				return;
			}
		}
	}

	public bool AreObjectivesActive()
	{
		// If round is not active, objectives can't be interacted with.
		if ( !IsRoundActive )
			return false;

		// if we're waiting for players, we can't cap.
		if ( IsWaitingForPlayers )
			return false;

		// same if we are in setup time
		if ( IsInSetup )
			return false;

		return true;
	}

	public override void SimulateGameplay()
	{
		base.SimulateGameplay();

		if ( !Game.IsServer )
			return;

		CheckWinConditions();
	}

	public void CheckWinConditions()
	{
		var mode = GetGamemode();
		if ( mode == default )
			return;

		if ( mode.HasWon( out var team, out var reason ) )
		{
			DeclareWinner( team, reason );
		}
	}

	public void DeclareWinner( Team team, WinReason reason )
	{
		DeclareWinner( (int)team, (int)reason );
	}
}