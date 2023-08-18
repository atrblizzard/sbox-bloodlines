using System;
using System.Text.RegularExpressions;

#if OLD

namespace Sandbox
{
	[Library( "ambient_generic" )]
	public partial class GenericAmbient : Entity
	{
		[Flags]
		public enum Flags
		{
			PlayEverywhere = 1,
			StartSilent = 16,
			IsNotLooped = 32,
		}

		/// <summary>
		/// Settings that are only applicable when the entity spawns
		/// </summary>
		[Property( "spawnflags", Title = "Spawn Settings")]
		[FGDType("flags")]
		public new Flags SpawnFlags { get; set; } = Flags.PlayEverywhere;

		/// <summary>
		/// Name of the GameSound entry for the sound to play. Also supports direct .wav filenames.
		/// </summary>
		[Property( "message", Title = "Sound Name" )]
		[FGDType("sound")]
        public string SoundName { get; set; }

		/// <summary>
		/// Sound volume, expressed as a range from 0 to 10, where 10 is the loudest.
		/// </summary>
		[Property( "health", Title = "Volume" )]
		public int VolumeValue { get; set; } = 10;

		/// <summary>
		/// Maximum distance at which this sound is audible.
		/// </summary>
		[Property("radius", Title = "Max Audible Distance" )]
		public string Radius { get; set; } = "1250";

		/// <summary>
		/// The entity to use as the origin of the sound playback. If not set, will play from this snd_event_point.
		/// </summary>
		[Property( "sourceEntityName")]
        [FGDType("target_destination")]
		public string SourceEntityName { get; set; }

		public Sound PlayingSound { get; protected set; }

		//Sound? PlayingSound = null;

		public bool IsPlaying { get; set; }

		public override void Spawn()
		{
			if ( !SpawnFlags.HasFlag( Flags.StartSilent ) )
			{
				PlaySound();
			}

			base.Spawn();
		}

		/// <summary>
		/// Start the sound event. If an entity name is provided, the sound will originate from that entity
		/// </summary>
		[Input]
		protected void PlaySound()
		{
			var source = FindByName( SourceEntityName, this );

			if ( !string.IsNullOrEmpty( SoundName ) )
			{
				if ( SpawnFlags.HasFlag( Flags.PlayEverywhere ) )
				{
					PlayingSound = Sound.FromWorld( SoundName, Position );
				}
				else
				{
					if ( source != null )
						PlayingSound = Sound.FromWorld( SoundName, source.Position );
					else
						PlayingSound = Sound.FromEntity( SoundName, this );
				}
			}

			PlayingSound.SetVolume( VolumeValue );

			IsPlaying = true;
		}

		/// <summary>
		/// Sets the sound volume, expressed as a range from 0 to 10, where 10 is the loudest.
		/// </summary>
		[Input]
		protected void Volume(int volume)
		{
			PlayingSound.SetVolume(volume / 10);
		}

		/// <summary>
		/// Stop the sound event
		/// </summary>
		[Input]
		protected void StopSound()
		{
			PlayingSound.Stop();
			PlayingSound = default;
			IsPlaying = false;
		}

        public override void OnClientActive(IClient cl)
        {
			if ( !SpawnFlags.HasFlag(Flags.StartSilent) )
			{
				PlaySound();
			}	
		}

		[Input]
		protected void ToggleSound()
		{
			if (!IsPlaying)
			{
				PlaySound();
			}
			else
			{
				StopSound();
			}
		}
	}
}
#endif