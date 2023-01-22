using Editor;
using Sandbox;

namespace bloodlines.entities.vampire.Events
{
	[HammerEntity]
	[Library( "events_world" )]
	public partial class WorldEvents : Entity
	{
		#region Outputs
		/// <summary>
		/// Fired when the cops outside.
		/// </summary>
		protected Output OnCopsOutside { get; set; }

		/// <summary>
		/// Fired when the cops are coming.
		/// </summary>
		protected Output OnCopsComing { get; set; }

		/// <summary>
		/// Fired when cop pursuit mode stars.
		/// </summary>
		protected Output OnStartCopPursuitMode { get; set; }

		/// <summary>
		/// Fired when cop pursuit mode ends.
		/// </summary>
		protected Output OnEndCopPursuitMode { get; set; }

		/// <summary>
		/// Fired when cop alert mode starts.
		/// </summary>
		protected Output OnStartCopAlertMode { get; set; }

		/// <summary>
		/// Fired when cop alert mode ends.
		/// </summary>
		protected Output OnEndCopAlertMode { get; set; }

		/// <summary>
		/// Fired when hunter alert mode starts.
		/// </summary>
		protected Output OnStartHunterPursuitMode { get; set; }

		/// <summary>
		/// Fired when hunter alert mode ends.
		/// </summary>
		protected Output OnEndHunterPursuitMode { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 1 point.
		/// </summary>
		protected Output OnMasqueradeLevel1 { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 2 point.
		/// </summary>
		protected Output OnMasqueradeLevel2 { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 3 point.
		/// </summary>
		protected Output OnMasqueradeLevel3 { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 4 point.
		/// </summary>
		protected Output OnMasqueradeLevel4 { get; set; }

		/// <summary>
		/// Fired when player's masquerade level reaches 5 point.
		/// </summary>
		protected Output OnMasqueradeLevel5 { get; set; }

		/// <summary>
		/// Fired when player's masquerade level changed.
		/// </summary>
		protected Output OnMasqueradeLevelChanged { get; set; }

		/// <summary>
		/// Fired when player needs blood.
		/// </summary>
		protected Output OnPlayerHasNoBlood { get; set; }

		/// <summary>
		/// Fired when combat music starts playing.
		/// </summary>
		protected Output OnCombatMusicStart { get; set; }

		/// <summary>
		/// Fired when combat music stops playing.
		/// </summary>
		protected Output OnCombatMusicEnd { get; set; }

		/// <summary>
		/// Fired when alert music starts playing.
		/// </summary>
		protected Output OnAlertMusicStart { get; set; }

		/// <summary>
		/// Fired when alert music stops playing.
		/// </summary>
		protected Output OnAlertMusicEnd { get; set; }

		/// <summary>
		/// Fired when normal music starts playing.
		/// </summary>
		protected Output OnNormalMusicStart { get; set; }

		/// <summary>
		/// Fired when normal music stops playing.
		/// </summary>
		protected Output OnNormalMusicEnd { get; set; }
		#endregion

		#region Inputs

		/// <summary>
		/// Sets area safety type.
		/// </summary>
		/// <param name="type"></param>
		[Input]
		public void SetSafeArea( int type )
		{
			Log.Info( $"Setting SetSafeArea to value {type}" );
		}

		/// <summary>
		/// Sets cop waiting area.
		/// </summary>
		/// <param name="area"></param>
		[Input]
		public void SetCopWaitArea( int area )
		{
			Log.Info( $"Setting SetCopWaitArea to value {area}" );
		}

		/// <summary>
		/// Sets cop grace.
		/// </summary>
		/// <param name="grace"></param>
		[Input]
		public void SetCopGrace( int grace )
		{
			Log.Info( $"Setting SetCopGrace to value {grace}" );
		}

		/// <summary>
		/// Sets nosferatu-tolerant area.
		/// </summary>
		/// <param name="tolerancy"></param>
		[Input]
		public void SetNosferatuTolerant( int tolerancy )
		{
			Log.Info( $"Setting SetNosferatuTolerant to value {tolerancy}" );
		}

		/// <summary>
		/// Sets this area as frenzy-impossible.
		/// </summary>
		/// <param name="area"></param>
		[Input]
		public void SetNoFrenzyArea( int area )
		{
			Log.Info( $"Setting SetNoFrenzyArea to value {area}" );
		}

		/// <summary>
		/// Hides cutscene-related interfering entities.
		/// </summary>
		[Input]
		public void HideCutsceneInterferingEntities()
		{
			Log.Info( $"Calling HideCutsceneInterferingEntities input" );
		}

		/// <summary>
		/// Hides cutscene-related interfering entities.
		/// </summary>
		[Input]
		public void UnhideCutsceneInterferingEntities()
		{
			Log.Info( $"Calling UnhideCutsceneInterferingEntities input" );
		}

		/// <summary>
		/// Fade global wetness (on rain start or end).
		/// </summary>
		/// <param name="wetness"></param>
		[Input]
		public void FadeGlobalWetness( int wetness )
		{
			Log.Info( $"Calling FadeGlobalWetness input with wetness parameter and {wetness} value." );
		}

		/// <summary>
		/// Show end-game credits.
		/// </summary>
		[Input]
		public void PlayEndCredits()
		{
			Log.Info( $"Calling PlayEndCredits input." );
		}
		#endregion

		public override void Spawn()
		{
			base.Spawn();
		}
	}
}
