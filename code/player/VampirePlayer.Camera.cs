using Amper.FPS;
using Sandbox;

namespace Vampire;

partial class VampirePlayer
{
	[Net, Predicted]
	public bool IsThirdPersonEnabled { get; set; } // Cannot override base Thirdperson

	public override bool IsThirdPerson => IsThirdPersonEnabled;
	
	private bool StayInThirdPerson { get; set; }
	private bool WasFirstPerson { get; set; }
	
	public override void CalculatePlayerView()
	{
		Camera.Rotation = ViewAngles.ToRotation();

		if ( IsThirdPersonEnabled )
		{
			Camera.FirstPersonViewer = null;

			var center = Position + Vector3.Up * 64;
			DebugOverlay.Axis( center, Rotation );

			var pos = center;
			var rot = ViewAngles.ToRotation();

			float distance = cl_thirdperson_distance * Scale;
			//targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 32) * Scale);
			var targetPos = pos;
			targetPos += rot.Forward * -distance;

			var tr = Trace.Ray( pos, targetPos )
				.WithAnyTags( "solid" )
				.StaticOnly()
				.Radius( 8 )
				.Run();

			Camera.Position = tr.EndPosition;
		}
		else
		{
			Camera.Position = this.GetEyePosition();
			Camera.FirstPersonViewer = this;

			SmoothViewOnStairs();

			var punch = ViewPunchAngle;
			Camera.Rotation *= Rotation.From( punch.x, punch.y, punch.z );
			SmoothViewOnStairs();
		}
	}
	
	public void SimulateCameraLogic()
	{
		if ( Input.Pressed( InputButton.View) )
		{
			SwapCamera();
			StayInThirdPerson = IsThirdPersonEnabled;
		}
	}
	
	/// <summary>
	/// Changes camera from firstperson to thirdperson and vice-versa
	/// </summary>
	public void SwapCamera() => IsThirdPersonEnabled = !IsThirdPersonEnabled;

	/// <summary>
	/// Forces camera to thirdperson if true, firstperson if false
	/// </summary>
	/// <param name="enabled"></param>
	public void ThirdPersonSet( bool enabled ) => IsThirdPersonEnabled = enabled;
}