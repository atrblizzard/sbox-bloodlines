using Sandbox;

[Library( "logic_choreographed_scene" )]
//[Hammer.EditorModel( "models/editor/cone_helper.vmdl" )]
public partial class LogicChoreographedScene : Entity
{
	/// <summary>
	/// The VCD file to use.
	/// </summary>
	[Property( "SceneFile", Title = "Scene File" )]
	public string SceneFile { get; set; }

	/// <summary>
	/// If this entity's scene was interrupted and we plan on going back to it, tells actors to play this scene instead.
	/// </summary>
	[Property( "ResumeSceneFile", Title = "Resume Scene File" )]
	public string ResumeSceneFile { get; set; }

	/// <summary>
	/// Targetnames of actors which will take part in this scene. These should match with names defined in the VCD.
	/// </summary>
	[Property( "target1", Title = "Target 1" ), FGDType( "target_destination" )]
	public string Target1 { get; set; }

	[Property( "target2", Title = "Target 2" ), FGDType( "target_destination" )]
	public string Target2 { get; set; }

	[Property( "target3", Title = "Target 3" ), FGDType( "target_destination" )]
	public string Target3 { get; set; }

	[Property( "target4", Title = "Target 4" ), FGDType( "target_destination" )]
	public string Target4 { get; set; }

	/// <summary>
	/// What to do if an actor this scene needs is already talking when this scene is told to start.
	/// 0: Start immediately (Unnatural.)
	/// 1: Wait for actor to finish
	/// 2: Interrupt at next interrupt event - Actor waits until they're at an appropriate point to quit speaking to play this scene, then goes back to their previous one.
	/// 3: Cancel at next interrupt event - Actor waits until they're at an appropriate point to quit speaking to play this scene, then forgets about their previous one.
	/// </summary>
	[Property( "busyactor", Title = "If an Actor is talking..." )]
	public int BusyActor { get; set; }

	/// <summary>
	/// What should our actors do if the player dies?
	/// 0: Do Nothing
	/// 1: Cancel Script and return to AI
	/// </summary>
	[Property( "onplayerdeath", Title = "On player death" )]
	public int OnPlayerDeath { get; set; }

	[Property( "full_sound", Title = "Full Sound" )]
	public int FullSound { get; set; }

	[Property( "hide_ents", Title = "Hide Entities" )]
	public int HideEntities { get; set; }

	/// <summary>
	/// Starts playback of the scene file.
	/// </summary>
	[Input]
	public void Start()
	{
		Log.Info( "Start VCD scene" );
		// var targetEnt1 = FindByName( Target1 );
		// if (targetEnt1 != null)
		// 	Log.Info($"Found {targetEnt1}");
		// targetEnt1.

		if ( FindByName( Target1 ) is AnimatedEntity animEntity )
		{
			Log.Info( $"Found {Target1}" );

			//AnimSceneObject test = new AnimSceneObject( animEntity.GetModel(), Transform.Zero);


			if ( !string.IsNullOrEmpty( SceneFile ) )
			{
				animEntity.CurrentSequence.Name = null;
			}
		}

		OnStart.Fire( this );
	}

	/// <summary>
	/// Pauses playback of the scene file.
	/// </summary>
	[Input]
	public void Pause()
	{

	}

	/// <summary>
	/// Resumes playback of the scene if it was paused.
	/// </summary>
	[Input]
	public void Resume()
	{

	}

	/// <summary>
	/// Cancels playback of the scene.
	/// </summary>
	[Input]
	public void Cancel()
	{

	}

	/// <summary>
	/// Cancels playback of the scene at the next interrupt event in the scene.
	/// </summary>
	[Input]
	public void CancelAtNextInterrupt()
	{

	}

	/// <summary>
	/// Multiplies the pitch of all speech involved in the scene.
	/// </summary>
	[Input]
	public void PitchShift( float value )
	{

	}

	/// <summary>
	/// If not currently playing a scene, tells the entity to stop waiting on an actor to stop talking
	/// </summary>
	[Input]
	public void StopWaitingForActor()
	{

	}

	/// <summary>
	/// Makes the associated OnTrigger output fire.
	/// </summary>
	[Input]
	public void Trigger( int trigger )
	{
		switch ( trigger )
		{
			case 1:
				OnTrigger1.Fire( this );
				break;
			case 2:
				OnTrigger2.Fire( this );
				break;
			case 3:
				OnTrigger3.Fire( this );
				break;
			case 4:
				OnTrigger4.Fire( this );
				break;
		}
	}

	/// <summary>
	/// Sets the associated Target (x) keyvalue.
	/// </summary>
	[Input]
	public void SetTarget1( string target )
	{

	}

	[Input]
	public void SetTarget2( string target )
	{

	}

	[Input]
	public void SetTarget3( string target )
	{

	}

	[Input]
	public void SetTarget4( string target )
	{

	}

	protected Output OnStart { get; set; }
	protected Output OnCompletion { get; set; }
	protected Output OnCanceled { get; set; }
	protected Output OnTrigger1 { get; set; }
	protected Output OnTrigger2 { get; set; }
	protected Output OnTrigger3 { get; set; }
	protected Output OnTrigger4 { get; set; }
	protected Output OnTrigger5 { get; set; }
	protected Output OnTrigger6 { get; set; }
	protected Output OnTrigger7 { get; set; }
	protected Output OnTrigger8 { get; set; }
	protected Output OnTrigger9 { get; set; }
	protected Output OnTrigger10 { get; set; }
	protected Output OnTrigger11 { get; set; }
	protected Output OnTrigger12 { get; set; }
	protected Output OnTrigger13 { get; set; }
	protected Output OnTrigger14 { get; set; }
	protected Output OnTrigger15 { get; set; }
	protected Output OnTrigger16 { get; set; }
}
