using Sandbox;

namespace Bloodlines.VR.Player
{
	partial class VRPlayer : Sandbox.Player
	{
		ModelEntity gloves, helmet;

		[Net, Predicted] 
		AnimEntity LH { get; set; }

		[Net, Predicted] 
		AnimEntity RH { get; set; }

		[Net, Predicted] 
		AnimEntity PlayerPuppet { get; set; }

		bool GrabbedL = false, GrabbedR = false, GrabbedBoth = false, OverrideR, OverrideL;

		Vector3 GrabPos = new Vector3();
		Vector3 DynamicGrabPos = new Vector3();
		Rotation StartRot = new Rotation();
		Entity GrabbedEntity;

		bool IsDead = false;

		bool UseSeparateHands = true;

		int GrabbedLFrameCount = 0, GrabbedRFrameCount = 0;

		WalkControllerVR ControllerRef;
		StandardPlayerAnimatorVR AnimatorRef;

		public override void Respawn()
		{
			base.Respawn();
			SetModel( "models/citizen/citizen.vmdl" );

			//startrot = Rotation;

			//gloves = new ModelEntity();
			//gloves.SetModel( "models/gloves.vmdl" );
			//gloves.SetParent( this, true );
			//gloves.EnableShadowInFirstPerson = true;
			//gloves.EnableHideInFirstPerson = true;

			if ( GetClientOwner().IsUsingVr )
			{
				Controller = new WalkControllerVR();
				ControllerRef = Controller as WalkControllerVR;

				ControllerRef.BodyGirth = 12f;

				if ( Animator == null )
				{
					Animator = new StandardPlayerAnimatorVR();
					AnimatorRef = Animator as StandardPlayerAnimatorVR;
				}

				Camera = new FirstPersonCamera();

				EnableAllCollisions = true;
				EnableDrawing = false;
				EnableHideInFirstPerson = true;
				EnableShadowInFirstPerson = true;
				
			}
			else
			{
				Controller = new WalkController();

				Animator = new StandardPlayerAnimator();

				Camera = new FirstPersonCamera();

				EnableAllCollisions = true;
				EnableDrawing = true;
				EnableHideInFirstPerson = true;
				EnableShadowInFirstPerson = true;
			}

			IsDead = false;
		}

		public override void PostCameraSetup( ref CameraSetup setup )
		{
			base.PostCameraSetup( ref setup );
			if ( PlayerPuppet != null )
			{
				PlayerPuppet.SetBodyGroup( "head", 1 );
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();

			if ( RH != null )
			{
				RH.Delete();
			}

			if ( LH != null )
			{
				LH.Delete();
			}

			BecomeRagdollOnClient(Controller.Velocity, 0);

			Controller = null;
			Camera = new SpectateRagdollCamera();

			IsDead = true;
			EnableAllCollisions = false;
			EnableDrawing = false;			
		}

		public void DoVRGrabbing()
		{
			Transform LocalLeftHand = Transform.ToLocal( Input.VR.LeftHand.Transform );
			Transform LocalRightHand = Transform.ToLocal( Input.VR.RightHand.Transform );

			if (GrabbedL )
			{
				if (Controller.Pawn.GroundEntity != null )
				{
					Controller.Pawn.GroundEntity = null;
				}

				Vector3 DeltaVel = GrabPos - ((LocalLeftHand.Position * Rotation) + Position);

				Controller.Pawn.Velocity = DeltaVel* Time.Delta * 1500;


				DebugOverlay.Text(GrabPos, "Grabbing!" );
			}

			Trace LeftHandTrace = Trace.Ray( Input.VR.LeftHand.Transform.Position, Input.VR.LeftHand.Transform.Position + Input.VR.LeftHand.Transform.Rotation.Forward * 4f ).Radius( 5f );

			LeftHandTrace.Ignore( Root );
			TraceResult LeftHandResult = LeftHandTrace.Run();

			if ( LeftHandResult.Hit )
			{
				DebugOverlay.Circle( LeftHandResult.EndPos + LeftHandResult.Normal * 0.01f, Rotation.LookAt( LeftHandResult.Normal ), 1f, Color.Green );
			}

			if ( LeftHandResult.Hit && Input.VR.LeftHand.Trigger.Value > 0.1f && !GrabbedL && !OverrideL )
			{
				if ( GrabbedR )
				{
					OverrideR = true;
					GrabbedR = false;
				}

				GrabbedL = true;
				GrabPos = (LocalLeftHand.Position * Rotation) + Position;
			}

			if ( GrabbedR )
			{
				if ( Controller.Pawn.GroundEntity != null )
				{
					Controller.Pawn.GroundEntity = null;
				}

				Vector3 DeltaVel = GrabPos - ((LocalRightHand.Position * Rotation) + Position);

				Controller.Pawn.Velocity = DeltaVel * Time.Delta * 1500;


				DebugOverlay.Text( GrabPos, "Grabbing!" );
			}

			Trace RightHandTrace = Trace.Ray( Input.VR.RightHand.Transform.Position, Input.VR.RightHand.Transform.Position + Input.VR.RightHand.Transform.Rotation.Forward * 4f ).Radius(5f);

			RightHandTrace.Ignore( Root );
			TraceResult RightHandResult = RightHandTrace.Run();

			if ( RightHandResult.Hit )
			{
				DebugOverlay.Circle( RightHandResult.EndPos + RightHandResult.Normal * 0.01f, Rotation.LookAt( RightHandResult.Normal ), 1f, Color.Green );
			}

			if ( RightHandResult.Hit && Input.VR.RightHand.Trigger.Value > 0.1f && !GrabbedR && !OverrideR )
			{
				if ( GrabbedL )
				{
					OverrideL = true;
					GrabbedL = false;
				}
				GrabbedR = true;
				GrabPos = (LocalRightHand.Position * Rotation) + Position;
			}

			if ( Input.VR.LeftHand.Trigger.Value < 0.1f )
			{
				OverrideL = false;
				GrabbedL = false;
				GrabbedEntity = null;
			}

			if ( Input.VR.RightHand.Trigger.Value < 0.1f )
			{
				OverrideR = false;
				GrabbedR = false;
				GrabbedEntity = null;
			}

			if ( (Input.VR.RightHand.Trigger.Value > 0.1f && Input.VR.LeftHand.Trigger.Value > 0.1f) &&
				!GrabbedBoth && Controller.Pawn.GroundEntity != null )
			{
				GrabbedBoth = true;
				GrabPos = ((LocalRightHand.Position + LocalLeftHand.Position) / 2f * Rotation) + Position;
			}

			if ( GrabbedBoth )
			{
				if ( Controller.Pawn.GroundEntity != null )
				{
					Controller.Pawn.GroundEntity = null;
				}

				Vector3 DeltaVel = GrabPos - (((LocalRightHand.Position + LocalLeftHand.Position) / 2f * Rotation) + Position);

				Controller.Pawn.Velocity = DeltaVel * Time.Delta * 1500;

				DebugOverlay.Text( GrabPos, "Grabbing!" );
			}

			if ( Input.VR.RightHand.Trigger.Value < 0.1f && Input.VR.LeftHand.Trigger.Value < 0.1f && GrabbedBoth )
			{
				GrabbedBoth = false;
			}

			if ( ControllerRef == null )
			{
				ControllerRef = Controller as WalkControllerVR;
			}

			ControllerRef.Climbing = GrabbedBoth || GrabbedR || GrabbedL;
		}

		float TimeSinceFootShuffle;

		public void DoPuppetRotation( Rotation idealRotation )
		{
			var allowYawDiff = 30f;

			float turnSpeed = 0.01f;

			PlayerPuppet.Rotation = Rotation.Slerp( PlayerPuppet.Rotation, idealRotation, Time.Delta * turnSpeed );
			PlayerPuppet.Rotation = PlayerPuppet.Rotation.Clamp( idealRotation, allowYawDiff, out var change );

			if ( change > 1 )
				TimeSinceFootShuffle = 0;

			PlayerPuppet.SetAnimBool( "b_shuffle", TimeSinceFootShuffle < 0.1 );
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		/// 
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			TimeSinceFootShuffle += Time.Delta;

			if ( IsServer )
			{
				if ( Input.VR.IsActive )
				{
					Transform LocalHead = Transform.ToLocal( Input.VR.Head );
					if ( AnimatorRef == null )
					{
						Animator = new StandardPlayerAnimatorVR();
						AnimatorRef = Animator as StandardPlayerAnimatorVR;
					}

					if ( PlayerPuppet == null )
					{
						PlayerPuppet = new AnimEntity();
						PlayerPuppet.SetModel( "models/citizen/citizen.vmdl" );
						PlayerPuppet.Parent = Local.Pawn;
						AnimatorRef.PlayerPuppet = PlayerPuppet;
						PlayerPuppet.EnableShadowInFirstPerson = true;
						PlayerPuppet.EnableHideInFirstPerson = true;						
					}

					PlayerPuppet.EnableShadowInFirstPerson = true;
					PlayerPuppet.EnableHideInFirstPerson = true;

					if ( helmet == null )
					{
						helmet = new ModelEntity();
						helmet.SetModel( "models/helmet.vmdl" );
						helmet.SetParent( PlayerPuppet, "head" );
						helmet.EnableShadowInFirstPerson = true;
						helmet.EnableHideInFirstPerson = true;

						helmet.Position = PlayerPuppet.GetBoneTransform( "head" ).Position + Vector3.Down * 56.5f + Vector3.Forward * 3f;
					}

					if ( LH == null )
					{
						LH = new AnimEntity();
						LH.SetModel( "models/handleft.vmdl" );
					}

					if ( RH == null )
					{
						RH = new AnimEntity();
						RH.SetModel( "models/handright.vmdl" );
					}

					LH.Transform = Input.VR.LeftHand.Transform;
					RH.Transform = Input.VR.RightHand.Transform;

					LH.Position += Controller.Pawn.Velocity * Time.Delta * 2.25f;
					RH.Position += Controller.Pawn.Velocity * Time.Delta * 2.25f;

					LH.SetAnimFloat( "Thumb", Input.VR.LeftHand.GetFingerValue( FingerValue.ThumbCurl ) );
					LH.SetAnimFloat( "Index", Input.VR.LeftHand.GetFingerValue( FingerValue.IndexCurl ) );
					LH.SetAnimFloat( "Middle", Input.VR.LeftHand.GetFingerValue( FingerValue.MiddleCurl ) );
					LH.SetAnimFloat( "Ring", Input.VR.LeftHand.GetFingerValue( FingerValue.RingCurl ) );

					RH.SetAnimFloat( "Thumb", Input.VR.RightHand.GetFingerValue( FingerValue.ThumbCurl ) );
					RH.SetAnimFloat( "Index", Input.VR.RightHand.GetFingerValue( FingerValue.IndexCurl ) );
					RH.SetAnimFloat( "Middle", Input.VR.RightHand.GetFingerValue( FingerValue.MiddleCurl ) );
					RH.SetAnimFloat( "Ring", Input.VR.RightHand.GetFingerValue( FingerValue.RingCurl ) );

					if ( AnimatorRef == null )
					{
						AnimatorRef = Animator as StandardPlayerAnimatorVR;
					}
					if ( PlayerPuppet == null )
					{
						PlayerPuppet = new AnimEntity();
						PlayerPuppet.SetModel( "models/citizen/citizen.vmdl" );
						PlayerPuppet.Parent = Local.Pawn;
						AnimatorRef.PlayerPuppet = PlayerPuppet;
						PlayerPuppet.EnableShadowInFirstPerson = true;
						PlayerPuppet.EnableHideInFirstPerson = true;

						//HideHeadgroup( true );
					}
					if ( AnimatorRef != null )
					{
						AnimatorRef.PlayerPuppet = PlayerPuppet;
					}

					EnableDrawing = false;
					EnableShadowInFirstPerson = false;

					PlayerPuppet.SetBodyGroup( 3, 1 );

					Sandbox.VR.Scale = 1f;

					PlayerPuppet?.SetAnimBool( "b_vr", true );

					Angles puppetAng = Input.VR.Head.Rotation.Angles();
					puppetAng.roll = 0;
					puppetAng.pitch = 0;

					//PlayerPuppet.Rotation = puppetAng.ToRotation();

					DoPuppetRotation( puppetAng.ToRotation() );

					Vector3 HeadOffset = new Vector3( -10 - (((1 - (LocalHead.Position.z / 65f)) * 3f) * 10f), 0, 0 );
					PlayerPuppet.Position = Position + (LocalHead.Position.WithZ( 0 ) * Rotation) + (HeadOffset * PlayerPuppet.Rotation);

					PlayerPuppet?.SetAnimVector( "left_hand_ik.position", PlayerPuppet.Transform.ToLocal( LH.GetBoneTransform( 0 ) ).Position );

					PlayerPuppet?.SetAnimVector( "right_hand_ik.position", PlayerPuppet.Transform.ToLocal( RH.GetBoneTransform( 0 ) ).Position );

					PlayerPuppet?.SetAnimRotation( "left_hand_ik.rotation", PlayerPuppet.Transform.ToLocal( LH.GetBoneTransform( 0 ) ).Rotation * new Angles(0,0,180).ToRotation() );

					PlayerPuppet?.SetAnimRotation( "right_hand_ik.rotation", PlayerPuppet.Transform.ToLocal( RH.GetBoneTransform( 0 ) ).Rotation );

					PlayerPuppet?.SetAnimFloat( "duck", (1 - (LocalHead.Position.z / 65f)) * 3f );

					DoVRGrabbing();
				}
			}

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( Camera is ThirdPersonCamera )
				{
					Camera = new FirstPersonCamera();
				}
				else
				{
					Camera = new ThirdPersonCamera();
				}
			}

			if (IsDead) {
				return;
            }

			if (Input.Pressed( InputButton.Use ) )
			{
				ShootEffects();

				Trace ShootTrace = Trace.Ray( Input.VR.LeftHand.Transform.Position, Input.VR.LeftHand.Transform.Position + Input.VR.LeftHand.Transform.Rotation.Forward * 1000f );
				ShootTrace.Ignore( Root );
				TraceResult ShootResult = ShootTrace.Run();
				if( ShootResult.Hit && ShootResult.Entity.IsValid())
				{
					//DamageInfo damage = DamageInfo.FromBullet( ShootResult.EndPos, EyeRot.Forward * 100, 150);

					//ShootResult.Entity.TakeDamage( damage );
				}
			}

			if ( IsServer && Input.Pressed( InputButton.Menu ) )
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel( "models/block.vmdl" );
				ragdoll.Position = EyePos + EyeRot.Forward * 20;
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				ragdoll.Velocity = EyeRot.Forward * 500;
			}

			if ( IsServer && Input.Pressed( InputButton.Slot1 ) )
			{
				//HideHeadgroup();
			}

			if ( Input.Pressed( InputButton.Slot2 ) )
			{
				BecomeRagdollOnClient( Controller.Velocity * 0.1f, 0 );
			}

		}

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Host.AssertClient();

			Particles part = Particles.Create( "particles/pistol_muzzleflash.vpcf", EyePos );
			part.SetForward( 0, EyeRot.Forward );
		}

		DamageInfo LastDamage;

		public override void TakeDamage( DamageInfo info )
		{
			LastDamage = info;

			// hack - hitbox 0 is head
			// we should be able to get this from somewhere
			if ( info.HitboxIndex == 0 )
			{
				info.Damage *= 2.0f;
			}

			base.TakeDamage( info );

			if ( info.Attacker is VRPlayer attacker && attacker != this )
			{
				// Note - sending this only to the attacker!
				//attacker.DidDamage( attacker, info.Position, info.Damage, ((float)Health).LerpInverse( 100, 0 ) );
			}
		}

		[ClientRpc]
		public void DidDamage( Vector3 pos, float amount, float healthinv )
		{
			Sound.FromScreen( "dm.ui_attacker" )
				.SetPitch( 1 + healthinv * 1 );			
		}

		[ClientRpc]
		public void TookDamage( Vector3 pos )
		{
			DebugOverlay.Sphere( pos, 5.0f, Color.Red, false, 50.0f );

			DamageIndicator.Current?.OnHit( pos );
		}
	}
}
