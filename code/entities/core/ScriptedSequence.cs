using Sandbox;
using Editor;
using System.Threading.Tasks;


[Library( "scripted_sequence" )]
[Model( Archetypes = ModelArchetype.animated_model | ModelArchetype.static_prop_model )]
public partial class ScriptedSequence : Entity
{
	[Property( "Search Radius" )] public float m_flRadius { get; set; }
	[Property( "Repeat" )] public float m_flRepeat { get; set; }
	[Property( "Move to Position" )] public int m_fMoveTo { get; set; }

	/*
        [Property("Move to Position")] public int MoveTo
    {
        get => m_fMoveTo;
        set => m_fMoveTo = value;
    }

    private int m_fMoveTo;
     */


	//[Property("Entry Animation")] public string m_iszEntry { get; set; }
	//[Property] public string m_iszIdle { get; set; }

	[Property( "m_iszPlay", Title = "Play Sequence" )]
	public string PlaySequence { get; set; }

	//[Property] public string m_iszPostIdle { get; set; }
	//[Property] public string m_iszCustomMove { get; set; }
	//[Property("Next Script")] public string m_iszNextScript { get; set; } 

	//[Property] public string m_iszEntity { get; set; } 

	[Property( "target", Title = "Sequence Target" )]
	[FGDType( "target_destination" )]
	public string TargetEntity { get; set; }

	//[Property] public string targetname { get; set; }
	[Property( "m_bLoopActionSequence", Title = "Loop Action Animation? " )]
	public bool loopActionSequence { get; set; }

	[Property( "m_bSynchPostIdles", Title = "Synch Post Idles " )]
	public bool synchPostIdles { get; set; }

	[Property] public bool m_bIgnoreGravity { get; set; }
	[Property] public bool m_bDisableNPCCollisions { get; set; }

	[Input]
	public void MoveToPosition()
	{
	}

	[Input]
	public void BeginSequence()
	{
		Log.Info( "Beginning sequence" );

		//var targetEnt = FindByName( TargetEntity );

		if ( FindByName( TargetEntity ) is AnimatedEntity animEntity )
		{
			Log.Info( $"Found {TargetEntity}" );

			if ( !string.IsNullOrEmpty( PlaySequence ) )
			{
				if ( animEntity.CurrentSequence.Name == PlaySequence ) return;
				OnSequenceStarted( this );

				animEntity.CurrentSequence.Name = PlaySequence;
				Log.Info( $"Set sequence to \"{PlaySequence}\"" );

				Log.Info( animEntity.CurrentSequence.Duration );

				_ = EndSequenceTask( animEntity );
			}
		}
	}

	public async Task BeginSequenceTask( float timeDelay )
	{
		await Task.DelayRealtimeSeconds( timeDelay );
		OnSequenceStarted( this );
	}

	public async Task EndSequenceTask( AnimatedEntity currentEntity )
	{
		await Task.DelayRealtimeSeconds( currentEntity.CurrentSequence.Duration );
		OnSequenceEnded( this );
		currentEntity.CurrentSequence.Name = null;
	}

	public virtual void OnSequenceStarted( Entity other )
	{
		OnBeginSequence.Fire( other );
	}

	public virtual void OnSequenceEnded( Entity other )
	{
		OnEndSequence.Fire( other );
	}

	[Input]
	public void CancelSequence()
	{
	}

	[Input]
	public void ScriptPlayerDeath()
	{

	}

	protected Output OnBeginSequence { get; set; }
	protected Output OnEndSequence { get; set; }
	protected Output OnPostIdleEndSequence { get; set; }
	protected Output OnCancelSequence { get; set; }
	protected Output OnCancelFailedSequence { get; set; }
	protected Output OnScriptEvent01 { get; set; }
	protected Output OnScriptEvent02 { get; set; }
	protected Output OnScriptEvent03 { get; set; }
	protected Output OnScriptEvent04 { get; set; }
	protected Output OnScriptEvent05 { get; set; }
	protected Output OnScriptEvent06 { get; set; }
	protected Output OnScriptEvent07 { get; set; }
	protected Output OnScriptEvent08 { get; set; }
}
