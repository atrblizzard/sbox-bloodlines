using System;
using Editor;
using Sandbox;

namespace Vampire.entities.core.Sounds;

/// <summary>
/// Plays a sound event from a point. The point can be this entity or a specified entity's position.
/// </summary>
[Library("ambient_generic")]
[HammerEntity]
[EditorSprite("editor/ambient_generic.vmat")]
[VisGroup(VisGroup.Sound)]
[Title("ambient_generic"), Category("Sound"), Icon("volume_up")]
public partial class SoundEventEntity : Entity
{

    [Flags]
    public enum Flags
    {
        Playeverywhere = 1,
        StartSilent = 16,
        IsNOTLooped = 32,
    }

	[Property( "spawnflags", Title = "Spawn Settings" )]
	public Flags SpawnSettings { get; set; } = (Flags)48;
    /// <summary>
    /// Name of the sound to play.
    /// </summary>
    [Property("message")] //, FGDType("sound")]
    [Net] public string Message { get; set; }
	/// <summary>
	/// Volume of the sound
	/// </summary>
	[Property( "health" )]
	[Net] public float Volume { get; set; } = 10;

    /// <summary>
    /// The entity to use as the origin of the sound playback. If not set, will play from this snd_event_point.
    /// </summary>
    [Property("sourceEntityName"), FGDType("target_destination")]
    [Net] public string SourceEntityName { get; set; }

    /// <summary>
    /// Start the sound on spawn
    /// </summary>
    [Property("startOnSpawn")]
    [Net] public bool StartOnSpawn { get; set; }
    
    [Property("volstart", Title = "Volume Start")]
    [Net] public float VolumeStart { get; set; }

    /// <summary>
    /// Stop the sound before starting to play it again
    /// </summary>
    [Property("stopOnNew", Title = "Stop before repeat")]
    [Net] public bool StopOnNew { get; set; }

    public Sound PlayingSound { get; protected set; }
    public SoundFile EventSound;
    public SoundEventEntity()
    {
        Transmit = TransmitType.Always;
    }

    /// <summary>
    /// Start the sound event. If an entity name is provided, the sound will originate from that entity
    /// </summary>
    [Input]
    protected void PlaySound()
    {
        OnPlaySound();
    }

    /// <summary>
    /// Start the sound event. If an entity name is provided, the sound will originate from that entity
    /// </summary>
    [Input]
    protected void StartSound()
    {
        OnStartSound();
    }

    /// <summary>
    /// Stop the sound event
    /// </summary>
    [Input]
    protected void StopSound()
    {
        OnStopSound();
    }

    [Input]
    protected void ToggleSound()
    {

		if ( SpawnSettings.HasFlag( Flags.IsNOTLooped ) )
		{
			OnStartSound();
			return;
		}
		if ( HasStartedPlaying )
			OnStopSound();
		else OnStartSound();
    }

    [Input]
    void Kill()
    {
        if (Game.IsServer)
            Delete();
    } 
    public override void Spawn()
    {
        if (StartOnSpawn  || !SpawnSettings.HasFlag(Flags.StartSilent)) //broken on c1a0!!!!!!!!!!!!
        {
            StartSound();
        }
    }
    int ticker = 0;
    protected void OnStartSound()
    {
		HasStartedPlaying = true;
		OnPlaySound();

	}
	private void OnPlaySound()
	{
		var source = FindByName( SourceEntityName, this );

		if ( StopOnNew )
		{
			PlayingSound.Stop();
			PlayingSound = default;
		}

		if (PlayingSound.IsPlaying) return;
		
		var replaceName = Message;
		//replaceName = replaceName.Replace( "sounds/", "sounds/hl1/" );
		replaceName = replaceName.Replace( ".vsnd", ".sound" );
		replaceName = replaceName.Replace( "!", "" );

		return;
		Log.Info( $"starting sound {replaceName}" );
		//Sound.FromScreen(message);
		//PlayingSound = Sound.FromEntity( message, source );
		//EventSound = SoundFile.Load(replacename);

		PlayingSound = SpawnSettings.HasFlag( Flags.Playeverywhere ) ? Sound.FromScreen( replaceName ).SetVolume( Volume * 0.1f )
			: Sound.FromWorld( replaceName, Position ).SetVolume( Volume * 0.1f );
	}

    [ClientRpc]
    private void OnStopSound()
	{
		HasStartedPlaying = false;
		PlayingSound.Stop();
        PlayingSound = default;
    }
    
	bool HasStartedPlaying;

	
    [GameEvent.Tick.Server]
    public void Tick()
    {
		if (!HasStartedPlaying) return;
		if ( SpawnSettings.HasFlag( Flags.IsNOTLooped ) ) return;
		if ( PlayingSound.IsPlaying == false )
        {
            PlayingSound = default;
            OnStartSound();
        }
    }
}
