using System;
using System.Linq;
using Amper.FPS;
using Sandbox;

namespace Vampire;

public partial class VampirePlayer
{
	[ConVar.Replicated] public static bool vampire_class_change_instant_respawn { get; set; }
	
	[Net] public int ClanChanges { get; set; }
	[Net] public PlayerClan CurrentPlayerClan { get; set; }
	[Net] public PlayerClan DesiredPlayerClan { get; set; }

	private void SetupPlayerClass()
	{
		// We didn't choose any class. We can't regenerate.
		if ( !CurrentPlayerClan.IsValid() ) 
			return;

		// Change the model 
		if (!string.IsNullOrEmpty(CurrentPlayerClan.Model))
		{
			SetModel(CurrentPlayerClan.Model);
		}
		else
		{
			SetModel( "models/citizen/citizen.vmdl" );
			Log.Info( $"PlayerClass.Model is null or empty!" );
		}
		
		CurrentSequence.Name = "tremere_female_idle2";
		EnableShadowCasting = true;
	}
	
	public void SetClass( PlayerClan pclass )
	{
		var lastClass = CurrentPlayerClan;
		if ( lastClass == pclass )
			return;

		ClanChanges++;
		DesiredPlayerClan = pclass;
		VampireGameRules.Current.PlayerChangeClass( this, pclass );

		var shouldRespawn = lastClass == null;

		if ( shouldRespawn )
		{
			Respawn();
			return;
		}

		CommitSuicide( false );
		//TFChatBox.AddInformation( To.Single( Client ), $"* You will respawn as {pclass.Title}" );
		Log.Info( $"{Client.Name} changed their class {(lastClass != null ? $"from {lastClass.Title} " : "")}to {pclass.Title}" );
	}

	public void SetRandomClass()
	{
		// all classes minus undefined.
		var count = Enum.GetValues( typeof( VampirePlayerClan ) ).Length - 1;
		var random = Game.Random.Int( 0, count - 1 );
		var pclass = PlayerClan.Get( (VampirePlayerClan)random );

		if ( pclass == null )
		{
			Log.Info( $"SetRandomClass() - Failed to compute random class." );
			return;
		}

		SetClass( pclass );
	}

	#region Console Commands
	[ConCmd.Server( "join_clan" )]
	public static void Command_SetClass( string name )
	{
		if ( ConsoleSystem.Caller.Pawn is not VampirePlayer player ) return;

		// Don't allow class change if the game is over.
		if ( VampireGameRules.Current.State == GameState.GameOver )
			return;

		// If we are not playing as a playable team, don't allow changing classes.
		if (!player.Team.IsPlayable())
		{
			Log.Info("Team isn't playable!");
			return;
		}

		if (string.IsNullOrEmpty(name))
		{
			name = Game.Random.FromList( PlayerClan.All.Select( kv => kv.Key ).ToList() );
		}

		// assume all classes are lowercase (SOLDIER, Soldier and soldier are all one class.)
		name = name.ToLower();

		// see if we've chosen to select a random class.
		bool selectRandom = name is "auto" or "random";

		if( selectRandom )
		{
		 	player.SetRandomClass();
		 	return;
		} 

		// Currently nothing ever stops players from choosing any class.
		var pclass = PlayerClan.Get( name );

		player.SetClass( pclass );
	}
	#endregion
}