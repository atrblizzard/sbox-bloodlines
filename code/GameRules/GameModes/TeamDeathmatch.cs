using Sandbox;
using Editor;
using Amper.FPS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vampire;

[Library( "vampire_logic_tdm" )]
[Title( "Team Deathmatch" )]
[Category( "Gamemode" )]
[Icon( "groups" )]
[HammerEntity]

public partial class TeamDeathmatch : MapGamemode
{
	public override string Title => "Team Deathmatch";

	/// <summary>
	/// How many frags every team has collected.
	/// </summary>
	[Net] public IDictionary<Team, int> Frags { get; set; }

	/// <summary>
	/// This is the amount of time since either of the teams reached beep time.
	/// </summary>
	[Net] public TimeSince TimeSinceReachFragLimit { get; set; }

	/// <summary>
	/// This property will store the first team to reach frag limit.
	/// </summary>
	[Net] public Team FirstScorer { get; set; }
	[Net] public int FragLimit { get; set; }

	public TeamDeathmatch()
	{
		EventDispatcher.Subscribe<PlayerDeathEvent>( PlayerKilled, this );
	}

	public override bool HasWon( out Team team, out WinReason reason )
	{
		team = FirstScorer;
		reason = WinReason.FragLimit;

		return HasReachedFragLimit() && GetTimeUntilRoundEnd() == 0;
	}

	public override void Reset()
	{
		// reset frags
		Frags.Clear();
		FirstScorer = Team.Unassigned;

		// calculate the frag limit, based on the player count.
		float count = MathF.Ceiling( All.OfType<VampirePlayer>().Count() );
		var lerp = count.Remap( 4, 24 ).Clamp( 0, 1 );

		float min = v_tdm_frag_limit_min;
		float max = v_tdm_frag_limit_max;

		var limit = min.LerpTo( max, lerp ).FloorToInt();
		limit = Math.Max( limit, 1 );
		FragLimit = limit;
	}
	public virtual int GetTeamFragCount( Team team )
	{
		Frags.TryGetValue( team, out var count );
		return count;
	}

	public virtual float GetTimeUntilRoundEnd()
	{
		if ( !HasReachedFragLimit() )
			return v_tdm_finale_beep_time;

		var time = v_tdm_finale_beep_time - TimeSinceReachFragLimit;
		time = MathF.Max( 0, time );

		return time;
	}

	/// <summary>
	/// Are we currently in beep time?
	/// </summary>
	/// <returns></returns>
	public virtual bool HasReachedFragLimit()
	{
		// if ( !VampireGameRules.Current.AreObjectivesActive() )
		// 	return false;

		return Enum.GetValues( typeof( Team ) ).Cast<Team>()
			.Where( team => team.IsPlayable() )
			.Any( HasTeamReachedFragLimit );
	}

	public virtual bool HasTeamReachedFragLimit( Team team )
	{
		return GetTeamFragCount( team ) >= FragLimit;
	}

	public virtual void SetTeamFragCount( Team team, int count )
	{
		count = Math.Min( count, FragLimit );
		Frags[team] = count;
	}

	/// <summary>
	/// Add points to the team's frag score.
	/// </summary>
	public virtual void AddTeamFragCount( Team team, int count )
	{
		// Remember 
		bool wasNotInBeepTime = !HasReachedFragLimit();
		SetTeamFragCount( team, GetTeamFragCount( team ) + count );

		if ( wasNotInBeepTime && HasReachedFragLimit() )
		{
			FirstScorer = team;
			OnReachedFragLimit();
		}
	}

	public virtual void OnReachedFragLimit()
	{
		TimeSinceReachFragLimit = 0;
	}

	/// <summary>
	/// This is fired when a player dies.
	/// </summary>
	/// <param name="args"></param>
	public virtual void PlayerKilled( PlayerDeathEvent args )
	{
		if ( !Game.IsServer )
			return;

		// if ( !GameRules.Current.AreObjectivesActive() )
		// 	return;

		if ( args.Attacker is not ITeam attacker )
			return;

		var victim = args.Victim;

		if ( attacker == null )
			return;

		// Check if attacker and victim are not the same
		// we don't count suicides as frags.
		if ( attacker == victim )
			return;

		// Check if both attacker and victim are on different teams.
		if ( victim is ITeam teamVictim && attacker.TeamNumber == teamVictim.TeamNumber )
			return;

		// Get the attacker's team.
		var team = (Team)attacker.TeamNumber;

		// And give them one score.
		AddTeamFragCount( team, 1 );
	}

	[ConVar.Replicated] public static int v_tdm_frag_limit_min { get; set; } = 15;
	[ConVar.Replicated] public static int v_tdm_frag_limit_max { get; set; } = 75;
	[ConVar.Replicated] public static int v_tdm_finale_beep_time { get; set; } = 5;
}
