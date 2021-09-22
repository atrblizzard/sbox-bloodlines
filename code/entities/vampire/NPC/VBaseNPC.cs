using Sandbox;

namespace bloodlines.entities.vampire.NPC
{
	public partial class VBaseNPC : VAnimEntity
	{
		[Property( "base_gender" )]
		public int BaseGender { get; set; }

		[Property( "cantdropweapons" )]
		public int DontDropWeapons { get; set; }

		[Property( "invincible", "If set, the player can't kill this NPC.")]
		public int Invinsible { get; set; }

		protected Vector3 InputVelocity;
		protected Vector3 LookDir;


		[Input]
		public void SetHealth(int value)
		{

		}

		[Input]
		public void SetBodyGroup(int value)
		{

		}

		[Input]
		public void MoneyAdd()
		{

		}

		[Input]
		public void WillTalk(int value)
		{

		}

		protected Output OnDamaged { get; set; }
		protected Output OnDeath { get; set; }
		protected Output OnHalfHealth { get; set; }
		protected Output OnHearWorld { get; set; }
		protected Output OnHearPlayer { get; set; }
		protected Output OnHearCombat { get; set; }
		protected Output OnFoundEnemy { get; set; }
		protected Output OnLostEnemyLOS { get; set; }
		protected Output OnLostEnemy { get; set; }
		protected Output OnFoundPlayer { get; set; }
		protected Output OnLostPlayerLOS { get; set; }
		protected Output OnLostPlayer { get; set; }

		protected Output OnNPCDied { get; set; }
		protected Output OnLastNPCDied { get; set; }
		protected Output OnDialogBegin { get; set; }
		protected Output OnDialogEnd { get; set; }
		protected Output OnInterestingPlaceArrived { get; set; }
		protected Output OnInterestingPlaceLeft { get; set; }
		protected Output OnUnknownVisionPlayer { get; set; }
		protected Output OnStateFleeing { get; set; }
		protected Output OnIncapacitatedStart { get; set; }
		protected Output OnIncapacitatedEnd { get; set; }
		protected Output OnGrappleBegin { get; set; }
		protected Output OnGrappleEnd { get; set; }
		protected Output OnFedUponBegin { get; set; }
		protected Output OnFedUponEnd { get; set; }
		protected Output OnSellWeapon { get; set; }
		protected Output OnBarterClose { get; set; }

		public override void Spawn()
		{
			var model = GetModel();
			if ( model == null || model.IsError )
			{
				SetModel( "models/editor/playerstart.vmdl" );
				SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One, Vector3.One );
			}
			else
			{
				SetModel( GetModelName() );
				SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				CollisionGroup = CollisionGroup.Interactive;
				EnableSelfCollisions = true;
				MoveType = MoveType.MOVETYPE_WALK;
				EnableHitboxes = true;
				Velocity = Vector3.Zero;
			}

			var particleList = model.GetParticles();
			if ( particleList == null || particleList.Length <= 0 )
				return;

			foreach ( var particleData in particleList )
			{
				Particles.Create( particleData.Name, this, particleData.AttachmentPoint );
			}

			base.Spawn();
		}

		[Event.Tick.Server]
		public virtual void Tick()
		{
			InputVelocity = 0;

			Move( Time.Delta );

			var walkVelocity = Velocity.WithZ( 0 );
			if ( walkVelocity.Length > 0.5f )
			{
				var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100, true );
				var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
				Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
			}
			LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		}

		protected virtual void Move( float timeDelta )
		{
			var bbox = BBox.FromHeightAndRadius( 64, 4 );

			MoveHelper move = new( Position, Velocity );
			move.MaxStandableAngle = 50;
			move.Trace = move.Trace.Ignore( this ).Size( bbox );

			if ( !Velocity.IsNearlyZero( 0.001f ) )
			{
				move.TryUnstuck();
				move.TryMoveWithStep( timeDelta, 30 );
			}

			var tr = move.TraceDirection( Vector3.Down * 10.0f );

			if ( move.IsFloor( tr ) )
			{
				GroundEntity = tr.Entity;

				if ( !tr.StartedSolid )
				{
					move.Position = tr.EndPos;
				}

				if ( InputVelocity.Length > 0 )
				{
					var movement = move.Velocity.Dot( InputVelocity.Normal );
					move.Velocity -= movement * InputVelocity.Normal;
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
					move.Velocity += movement * InputVelocity.Normal;

				}
				else
				{
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				}
			}
			else
			{
				GroundEntity = null;
				move.Velocity += Vector3.Down * 900 * timeDelta;
			}

			Position = move.Position;
			Velocity = move.Velocity;
		}
	}
}
