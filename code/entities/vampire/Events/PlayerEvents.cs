using Editor;
using Sandbox;

namespace bloodlines.entities.vampire.Events
{
	[HammerEntity]
	[Library( "events_player" )]
	public partial class PlayerEvents : Entity
	{
		#region Outputs
		/// <summary>
		/// Fired when player's frenzy begins.
		/// </summary>
		protected Output OnFrenzyBegin { get; set; }

		/// <summary>
		/// Fired when the cops are coming.
		/// </summary>
		protected Output OnFrenzyEnd { get; set; }

		/// <summary>
		/// Fired when cop pursuit mode stars.
		/// </summary>
		protected Output OnWolfMorphBegin { get; set; }

		/// <summary>
		/// Fired when cop pursuit mode ends.
		/// </summary>
		protected Output OnWolfMorphEnd { get; set; }


		/// <summary>
		/// Fired when cop alert mode starts.
		/// </summary>
		protected Output OnPlayerTookDamage { get; set; }

		/// <summary>
		/// Fired when cop alert mode ends.
		/// </summary>
		protected Output OnPlayerKilled { get; set; }

		/// <summary>
		/// Fired when hunter alert mode starts.
		/// </summary>
		protected Output OnPlayerSoundLoud { get; set; }

		/// <summary>
		/// Fired when hunter alert mode ends.
		/// </summary>
		protected Output OnActivateAuspex { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 1 point.
		/// </summary>
		protected Output OnActivateCelerity { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 2 point.
		/// </summary>
		protected Output OnActivateCorpusVampirus { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 3 point.
		/// </summary>
		protected Output OnActivateFortitude { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 4 point.
		/// </summary>
		protected Output OnActivateObfuscate { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 5 point.
		/// </summary>
		protected Output OnActivatePotence { get; set; }

		/// <summary>
		/// Fired when player's masquerade level changed.
		/// </summary>
		protected Output OnActivatePresense { get; set; }

		/// <summary>
		/// Fired when player needs blood.
		/// </summary>
		protected Output OnActivateProtean { get; set; }

		/// <summary>
		/// Fired when combat music starts playing.
		/// </summary>
		protected Output OnActivateAnimalismLvl1 { get; set; }

		/// <summary>
		/// Fired when combat music stops playing.
		/// </summary>
		protected Output OnActivateAnimalismLvl2 { get; set; }

		/// <summary>
		/// Fired when alert music starts playing.
		/// </summary>
		protected Output OnActivateDementationLvl1 { get; set; }

		/// <summary>
		/// Fired when alert music stops playing.
		/// </summary>
		protected Output OnActivateDementationLvl2 { get; set; }

		/// <summary>
		/// Fired when normal music starts playing.
		/// </summary>
		protected Output OnActivateDominateLvl1 { get; set; }

		/// <summary>
		/// Fired when normal music stops playing.
		/// </summary>
		protected Output OnActivateDominateLvl2 { get; set; }

		/// <summary>
		/// Fired when normal music stops playing.
		/// </summary>
		protected Output OnActivateThaumaturgyLvl1 { get; set; }

		/// <summary>
		/// Fired when normal music stops playing.
		/// </summary>
		protected Output OnActivateThaumaturgyLvl2 { get; set; }

		#endregion

		#region Inputs

		/// <summary>
		/// Enables all entity's I/O connections
		/// </summary>
		[Input]
		public void EnableOutputs()
		{
			Log.Info( $"EnableOutputs" );
		}

		/// <summary>
		/// Disables all entity's I/O connections
		/// </summary>
		[Input]
		public void DisableOutputs( )
		{
			Log.Info( $"DisableOutputs" );
		}

		/// <summary>
		/// Creates NPC controller
		/// </summary>
		[Input]
		public void CreateControllerNPC()
		{
			Log.Info( $"CreateControllerNPC" );
		}

		/// <summary>
		/// Removes NPC controller
		/// </summary>
		[Input]
		public void RemoveControllerNPC( )
		{
			Log.Info( $"RemoveControllerNPC" );
		}

		/// <summary>
		/// Awards player with defined experience points.
		/// </summary>
		/// <param name="xp"></param>
		[Input]
		public void AwardExp( int xp )
		{
			Log.Info( $"Setting AwardExp to value {xp}" );
		}

		/// <summary>
		/// Hides cutscene-related interfering entities.
		/// </summary>
		[Input]
		public void ImmobilizePlayer()
		{
			Log.Info( $"Calling ImmobilizePlayer input" );
		}

		/// <summary>
		/// Hides cutscene-related interfering entities.
		/// </summary>
		[Input]
		public void MobilizePlayer()
		{
			Log.Info( $"Calling MobilizePlayer input" );
		}

		/// <summary>
		/// Removes defined disciplines
		/// </summary>
		[Input]
		public void RemoveDisciplines( )
		{
			Log.Info( $"Calling RemoveDisciplines input" );
		}

		/// <summary>
		/// Removes defeined disciplines right now 
		/// </summary>
		[Input]
		public void RemoveDisciplinesNow()
		{
			Log.Info( $"Calling RemoveDisciplinesNow input" );
		}

		/// <summary>
		/// Makes the player unkillable.
		/// </summary>
		[Input]
		public void MakePlayerUnkillable()
		{
			Log.Info( $"Calling MakePlayerUnkillable input" );
		}

		/// <summary>
		/// Returns the player to be killable.
		/// </summary>
		[Input]
		public void MakePlayerKillable()
		{
			Log.Info( $"Calling MakePlayerKillable input" );
		}

		#endregion

		public override void Spawn()
		{
			base.Spawn();
		}
	}
}
