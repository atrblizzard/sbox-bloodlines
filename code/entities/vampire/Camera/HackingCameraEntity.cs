using Sandbox;
using System;

[Library( "ent_hackingcamera", Title = "Hacking Camera" )]
public partial class HackingCameraEntity : Prop
{
	private readonly string camModel = "models/editor/camera.vmdl";

	//public ICamera ViewCamera;

	public override void Spawn()
	{
		SetModel( camModel );
		SetupPhysicsFromModel( PhysicsMotionType.Static, false );
	}

	[Event.Client.Frame]
	public void OnFrame()
	{
		//if ( this.Owner?.Camera is HackingCamera hc )
		//{
		//	//Yes, we do need to set this every frame
		//	//cameras really don't like having their properties set from the outside
		//	hc.Owner = this;
		//}

		//DebugOverlay.Text( this.Position, $"Owner: {this.Owner?.GetClientOwner().Name}", Color.Red, 0, 100 );
	}

	public void SetPhys( bool enable )
	{
		if ( enable )
		{
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, true );

			//The Weld tool won't work with this...
			//Make the camera unable to collide with player
			/*ClearCollisionLayers();
			AddCollisionLayer( CollisionLayer.Debris );*/
		}
		else
		{
			//Disable all physics, but still removeable with the remove tool
			PhysicsEnabled = false;
			//ClearCollisionLayers();
			//AddCollisionLayer( CollisionLayer.Debris );
		}
	}

	public override void OnKilled()
	{
		base.OnKilled();
	}

	[Event.Physics.PostStep]
	public void OnPostPhysicsStep()
	{
		if ( !this.IsValid() )
			return;

		var body = PhysicsBody;
		if ( !body.IsValid() )
			return;
	}

	protected override void OnDestroy()
	{
		//(this.Owner as VampirePlayer).MainCamera = new FirstPersonCamera();

		base.OnDestroy();
	}
}
