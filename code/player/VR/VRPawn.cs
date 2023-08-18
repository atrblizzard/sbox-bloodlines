using Sandbox;

namespace MyGame;

public partial class VRPawn : Entity
{
	[ClientInput] public Vector3 InputDirection { get; set; }
	[ClientInput] public Angles ViewAngles { get; set; }

	private readonly BBox _bbox = new(
		new Vector3( -16, -16, 0 ),
		new Vector3( 16, 16, 64 )
	);

	public BBox Hull { get; set; }

	private BBox GetHull()
	{
		var headLocal = Transform.ToLocal( Input.VR.Head );

		var mins = _bbox.Mins + headLocal.Position.WithZ( 0.0f ) * Rotation;
		var maxs = _bbox.Maxs + headLocal.Position.WithZ( 0.0f ) * Rotation;

		return new BBox( mins, maxs );
	}

	[BindComponent] public VRPawnController Controller { get; }

	[Net] public VRHandEntity LeftVrHand { get; set; }
	[Net] public VRHandEntity RightVrHand { get; set; }
	[Net] public VRHeadEntity VrHead { get; set; }

	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		//
		// Create a head so that other clients can see where we're looking
		//
		VrHead = new VRHeadEntity
		{
			Owner = this
		};
		VrHead.SetParent( this );

		//
		// Load the hands based on the prefabs inside the prefabs/hands/ directory.
		// You ideally shouldn't be changing this code unless you need to - the prefab
		// system will handle loading the correct model for the hand.
		//
		PrefabLibrary.TrySpawn<VRHandEntity>( "prefabs/hands/left.prefab", out var leftHand );
		leftHand.InputVrHand = VRHands.Left;
		leftHand.Owner = this;
		leftHand.SetParent( this );
		LeftVrHand = leftHand;

		PrefabLibrary.TrySpawn<VRHandEntity>( "prefabs/hands/right.prefab", out var rightHand );
		rightHand.InputVrHand = VRHands.Right;
		rightHand.Owner = this;
		rightHand.SetParent( this );
		RightVrHand = rightHand;
	}

	public void Respawn()
	{
		Components.Create<VRPawnController>();
	}

	public override void Simulate( IClient cl )
	{
		SimulateRotation();

		Hull = _bbox;

		LeftVrHand?.Simulate( cl );
		RightVrHand?.Simulate( cl );

		Controller?.Simulate( cl );
	}

	public override void BuildInput()
	{
		var analogValue = Input.VR.LeftHand.Joystick.Value;
		var headAngles = Input.VR.Head.Rotation.Angles();

		var moveRotation = Rotation.From( 0, headAngles.yaw, 0 );
		InputDirection = new Vector3( analogValue.y, -analogValue.x, 0 ) * moveRotation;

		if ( Input.StopProcessing )
			return;

		var look = Angles.Zero;

		if ( ViewAngles.pitch > 90f || ViewAngles.pitch < -90f )
		{
			look = look.WithYaw( look.yaw * -1f );
		}

		var viewAngles = ViewAngles;
		viewAngles += look;
		viewAngles.pitch = viewAngles.pitch.Clamp( -89f, 89f );
		viewAngles.roll = 0f;
		ViewAngles = viewAngles.Normal;
	}

	public override void FrameSimulate( IClient cl )
	{
		Camera.FirstPersonViewer = this;
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, float liftFeet = 0.0f )
	{
		return TraceBBox( start, end, Hull.Mins, Hull.Maxs, liftFeet );
	}

	private TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs, float liftFeet = 0.0f )
	{
		if ( liftFeet > 0 )
		{
			start += Vector3.Up * liftFeet;
			maxs = maxs.WithZ( maxs.z - liftFeet );
		}

		var tr = Trace.Ray( start, end )
					.Size( mins, maxs )
					.WithAnyTags( "solid", "playerclip", "passbullets" )
					.Ignore( this )
					.Run();

		return tr;
	}

	private TimeSince _timeSinceLastRotation;

	private void SimulateRotation()
	{
		const float Deadzone = 0.2f;
		const float Angle = 45f;
		const float Delay = 0.25f;

		float rotate = Input.VR.RightHand.Joystick.Value.x;

		if ( _timeSinceLastRotation > Delay )
		{
			if ( rotate > Deadzone )
			{
				Transform = Transform.RotateAround(
					Input.VR.Head.Position.WithZ( Position.z ),
					Rotation.FromAxis( Vector3.Up, -Angle )
				);

				_timeSinceLastRotation = 0;
			}
			else if ( rotate < -Deadzone )
			{
				Transform = Transform.RotateAround(
					Input.VR.Head.Position.WithZ( Position.z ),
					Rotation.FromAxis( Vector3.Up, Angle )
				);

				_timeSinceLastRotation = 0;
			}
		}

		if ( rotate is > -Deadzone and < Deadzone )
		{
			_timeSinceLastRotation = 10;
		}
	}
}