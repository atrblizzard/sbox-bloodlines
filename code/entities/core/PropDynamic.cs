using Editor;
using Sandbox;

[Library( "prop_dynamic" ), HammerEntity]
[Model ( Archetypes = ModelArchetype.physics_prop_model), RenderFields, VisGroup( VisGroup.Dynamic )]
[Title( "Prop" ), Category( "Gameplay" ), Icon( "chair" )]
public class PropDynamic : AnimatedMapEntity
{
    [Property] public bool Solid { get; set; } = true;


    private string sequence;
	public string Sequence
	{
		set
		{
			base.SetAnimation(value);
			sequence = value;
		}
		get
		{
			return sequence;
		}
	}

	[Property( "DefaultAnim" )]
	public string defaultAnim { get; set; }

	public override void Spawn()
	{
        PhysicsEnabled = false;
        UsePhysicsCollision = true;

        //Tags.Add(Solid ? CollisionTags.Solid : CollisionTags.NotSolid);

        SetupPhysicsFromModel( PhysicsMotionType.Static, false );

		//Log.Info( this.WorldAng.ToString() );
		//Log.Info( this.WorldRot.ToString() );
		this.Sequence = defaultAnim;
		this.PlaybackRate = 15f;

		base.Spawn();
	}

    [Input("SetMaterialGroupName")]
    private void Input_SetMaterialGroupName(string group)
    {
        SetMaterialGroup(group);
    }

    [Input("SetMaterialGroup")]
    private void Input_SetMaterialGroup(int group)
    {
        SetMaterialGroup(group);
    }

    [Input("SetBodyGroup")]
    private void Input_SetBodyGroup(int group)
    {
        SetBodyGroup(0, group);
    }

    /// <summary>
    /// Plays a specified animation sequence.
    /// </summary>
    [Input("SetAnimation")]
    private void Input_SetAnimation(string sequence)
    {
        CurrentSequence.Name = sequence;
        CurrentSequence.Time = 0f;
    }

    [Input("Disable")]
    private void Input_Disable()
    {
        EnableDrawing = false;
    }

    [Input("Enable")]
    private void Input_Enable()
    {
        EnableDrawing = true;
    }
}
