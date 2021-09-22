using System;

namespace Sandbox
{
	public class StandardPlayerAnimatorVR : StandardPlayerAnimator
	{
		TimeSince TimeSinceFootShuffle = 60;

		float duck;
		Vector2 RightJoy;
		bool JustRotated;

		public Angles PlayerRot;

		public AnimEntity PlayerPuppet;

		Vector3 OldHeadPos, NewHeadPos;

		/// <summary>
		/// Sets the param on the animgraph
		/// </summary>
		public override void SetParam( string name, Vector3 val )
		{
			PlayerPuppet?.SetAnimVector( name, val );
		}

		/// <summary>
		/// Sets the param on the animgraph
		/// </summary>
		public override void SetParam( string name, float val )
		{
			PlayerPuppet?.SetAnimFloat( name, val );
		}

		/// <summary>
		/// Sets the param on the animgraph
		/// </summary>
		public override void SetParam( string name, bool val )
		{
			PlayerPuppet?.SetAnimBool( name, val );
		}

		/// <summary>
		/// Sets the param on the animgraph
		/// </summary>
		public override void SetParam( string name, int val )
		{
			PlayerPuppet?.SetAnimInt( name, val );
		}

		public void UndoRotations()
		{
			Transform localHead = Pawn.Transform.ToLocal( Input.VR.Head );
			OldHeadPos = localHead.Position * Rotation;

			JustRotated = true;
			PlayerRot.yaw = 0;

			var idealRotation = Rotation.LookAt( PlayerRot.ToRotation().Forward.WithZ( 0 ), Vector3.Up );

			DoRotation( idealRotation );
		}

		public void HandleRotations()
		{
			Transform localHead = Pawn.Transform.ToLocal( Input.VR.Head );
			RightJoy = Input.VR.RightHand.Joystick.Value;

			if ( RightJoy.x > 0.5f && !JustRotated )
			{

				OldHeadPos = localHead.Position * Rotation;

				JustRotated = true;
				PlayerRot.yaw += -45;

			}

			if ( RightJoy.x < -0.5f && !JustRotated )
			{
				OldHeadPos = localHead.Position * Rotation;

				JustRotated = true;
				PlayerRot.yaw += 45;

			}

			if ( RightJoy.x < 0.5f && RightJoy.x > -0.5f && JustRotated )
			{
				JustRotated = false;
			}


			var idealRotation = Rotation.LookAt( PlayerRot.ToRotation().Forward.WithZ( 0 ), Vector3.Up );

			DoRotation( idealRotation );
		}

		public override void Simulate()
		{
			HandleRotations();

			DoWalk();

			//
			// Let the animation graph know some shit
			//
			bool sitting = HasTag( "sitting" );
			bool noclip = HasTag( "noclip" ) && !sitting;

			SetParam( "b_grounded", GroundEntity != null || noclip || sitting );
			SetParam( "b_noclip", noclip );
			SetParam( "b_sit", sitting );
			SetParam( "b_swim", Pawn.WaterLevel.Fraction > 0.5f && !sitting );

			Transform LocalHead = Pawn.Transform.ToLocal( Input.VR.Head );

			Vector3 aimPos = ((LocalHead.Position * Rotation) + Position) + ((LocalHead.Rotation.Forward * 100) * Rotation);//Pawn.EyePos + Pawn.EyeRot.Forward * 100;
			Vector3 lookPos = ((LocalHead.Position * Rotation) + Position) + ((LocalHead.Rotation.Forward * 100) * Rotation);

			//
			// Look in the direction what the player's input is facing
			//
			SetLookAt( "lookat_pos", lookPos ); // old
			SetLookAt( "aimat_pos", aimPos ); // old

			SetLookAt( "aim_eyes", lookPos );
			SetLookAt( "aim_head", lookPos );
			SetLookAt( "aim_body", aimPos );

			SetParam( "b_ducked", HasTag( "ducked" ) ); // old

			if ( HasTag( "ducked" ) ) duck = duck.LerpTo( 1.0f, Time.Delta * 10.0f );
			else duck = duck.LerpTo( 0.0f, Time.Delta * 5.0f );

			SetParam( "duck", duck );

			if ( Pawn.ActiveChild is BaseCarriable carry )
			{
				carry.SimulateAnimator( this );
			}
			else
			{
				SetParam( "holdtype", 0 );
				SetParam( "aimat_weight", 0.5f ); // old
				SetParam( "aim_body_weight", 0.5f );
			}


			if ( OldHeadPos != Vector3.Zero )
			{
				NewHeadPos = LocalHead.Position * Rotation;

				Position += (OldHeadPos - NewHeadPos);

				OldHeadPos = Vector3.Zero;
			}
		}

		public override void DoRotation( Rotation idealRotation )
		{
			//
			// Our ideal player model rotation is the way we're facing
			//
			var allowYawDiff = 60f;// Pawn.ActiveChild == null ? 90 : 50;

			float turnSpeed = 0.01f;
			//if ( HasTag( "ducked" ) ) turnSpeed = 0.1f;

			//Log.Trace( idealRotation.Angles().yaw );

			//
			// If we're moving, rotate to our ideal rotation
			//

			Rotation = idealRotation;//Rotation.Slerp( Rotation, , WishVelocity.Length * Time.Delta * turnSpeed );

			//
			// Clamp the foot rotation to within 120 degrees of the ideal rotation
			//
			Rotation = Rotation.Clamp( idealRotation, allowYawDiff, out var change );

			//
			// If we did restrict, and are standing still, add a foot shuffle
			//
			if ( change > 1 && WishVelocity.Length <= 1 ) TimeSinceFootShuffle = 0;

			SetParam( "b_shuffle", TimeSinceFootShuffle < 0.1 );
		}


		void DoWalk()
		{
			// Move Speed
			{
				var dir = Velocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "move_direction", angle );
				SetParam( "move_speed", Velocity.Length );
				SetParam( "move_groundspeed", Velocity.WithZ( 0 ).Length );
				SetParam( "move_y", sideward );
				SetParam( "move_x", forward );
			}

			// Wish Speed
			{
				var dir = WishVelocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "wish_direction", angle );
				SetParam( "wish_speed", WishVelocity.Length );
				SetParam( "wish_groundspeed", WishVelocity.WithZ( 0 ).Length );
				SetParam( "wish_y", sideward );
				SetParam( "wish_x", forward );
			}
		}

		public override void OnEvent( string name )
		{
			// DebugOverlay.Text( Pos + Vector3.Up * 100, name, 5.0f );

			if ( name == "jump" )
			{
				Trigger( "b_jump" );
			}

			base.OnEvent( name );
		}
	}
}
