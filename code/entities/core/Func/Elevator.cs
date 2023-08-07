using Sandbox;
using Editor;
using System;
using System.Threading.Tasks;

namespace bloodlines.entities.core.Func
{
	/// <summary>
	/// A brush entity that simulates the elevator functions.
	/// </summary>
	[Library( "func_elevator" )]
	[RenderFields]
	[DoorHelper( "movedir", "movedir_islocal", null, "movedistance" )]
	[HammerEntity]
	[Solid]
	public partial class Elevator : KeyframeEntity, IUse
	{
		/// <summary>
		/// The speed at which the door moves.
		/// </summary>
		[Property( "speed" )]
		public float Speed { get; set; } = 35;

		/// <summary>
		/// Defines height the 1st floor is (in units).
		/// </summary>
		[Property( "floor1" )]
		public float FloorHeight1 { get; set; } = 0;

		/// <summary>
		/// Defines height the 2nd floor is (in units).
		/// </summary>
		[Property( "floor2" )]
		public float FloorHeight2 { get; set; } = 0;

		/// <summary>
		/// Defines height the 3rd floor is (in units).
		/// </summary>
		[Property( "floor3" )]
		public float FloorHeight3 { get; set; } = 0;

		/// <summary>
		/// Defines height the 4th floor is (in units).
		/// </summary>
		[Property( "floor4" )]
		public float FloorHeight4 { get; set; } = 0;

		/// <summary>
		/// Defines height the 5th floor is (in units).
		/// </summary>
		[Property( "floor5" )]
		public float FloorHeight5 { get; set; } = 0;

		/// <summary>
		/// Defines height the 6th floor is (in units).
		/// </summary>
		[Property( "floor6" )]
		public float FloorHeight6 { get; set; } = 0;

		/// <summary>
		/// Defines height the 7th floor is (in units).
		/// </summary>
		[Property( "floor7" )]
		public float FloorHeight7 { get; set; } = 0;

		/// <summary>
		/// Defines height the 8th floor is (in units).
		/// </summary>
		[Property( "floor8" )]
		public float FloorHeight8 { get; set; } = 0;

		/// <summary>
		/// Defines the number of floors, used by elevator.
		/// </summary>
		[Property( "numfloors" )]
		public int NumFloors { get; set; } = 0;

		/// <summary>
		/// Be locked until output will sent by some action." = [ 0 : "No" 1 : "Yes" ]
		/// </summary>
		[Property( "locked" )]
		public bool Locked { get; set; }
		
		[Property( "startsound", Title = "Start Sound"), FGDType( "sound" )]
		public string StartSound { get; set; }
		[Property( "stopsound", Title = "Stop Sound"), FGDType( "sound" )]
		public string StopSound { get; set; }

		public float InitialPosition { get; set; } = 0;
		public bool MoveDirIsLocal { get; set; } = true;

		/// <summary>
		/// The direction the door will move, when it opens.
		/// </summary>
		[Property( "angles", Title = "Move Direction (Pitch Yaw Roll)" )]
		public Angles MoveDir { get; set; } = new Angles( -90, 0, 0 );

		public float MoveDistance { get; set; } = 100f;

		public bool IsMoving { get; protected set; }
		public bool IsMovingForwards { get; protected set; }

		#region Output
		protected Output OnMoveStart { get; set; }
		protected Output OnReachFloorAny { get; set; }
		protected Output OnPassFloorAny { get; set; }

		protected Output OnReachFloor1 { get; set; }
		protected Output OnReachFloor2 { get; set; }
		protected Output OnReachFloor3 { get; set; }
		protected Output OnReachFloor4 { get; set; }
		protected Output OnReachFloor5 { get; set; }
		protected Output OnReachFloor6 { get; set; }
		protected Output OnReachFloor7 { get; set; }
		protected Output OnReachFloor8 { get; set; }

		protected Output OnPassFloor1 { get; set; }
		protected Output OnPassFloor2 { get; set; }
		protected Output OnPassFloor3 { get; set; }
		protected Output OnPassFloor4 { get; set; }
		protected Output OnPassFloor5 { get; set; }
		protected Output OnPassFloor6 { get; set; }
		protected Output OnPassFloor7 { get; set; }
		protected Output OnPassFloor8 { get; set; }
		protected Output OnStop { get; set; }

		protected Output OnReachStart { get; set; }
		protected Output OnReachEnd { get; set; }
		protected Output OnMoveTowardsStart { get; set; }
		protected Output OnMoveTowardsEnd { get; set; }
		#endregion

		#region Inputs

		[Input]
		public void CallCurrentFloorOutputs()
		{

		}

		[Input]
		public void SnapToFloor()
		{

		}

		[Input]
		public void GotoFloor( int floor, Entity activator = null )
		{
			float floorDistance = 0;

			if ( floor > NumFloors )
			{
				Log.Warning( $"Elevator asked to go to invalid floor {floor}" );
				return;
			}

			switch ( floor )
			{
				case 1:
					floorDistance = FloorHeight1;
					OnPassFloor1.Fire( activator );
					break;
				case 2:
					floorDistance = FloorHeight2;
					OnPassFloor2.Fire( activator );
					break;
				case 3:
					floorDistance = FloorHeight3;
					OnPassFloor3.Fire( activator );
					break;
				case 4:
					floorDistance = FloorHeight4;
					OnPassFloor4.Fire( activator );
					break;
				case 5:
					floorDistance = FloorHeight5;
					OnPassFloor5.Fire( activator );
					break;
				case 6:
					floorDistance = FloorHeight6;
					OnPassFloor6.Fire( activator );
					break;
				case 7:
					floorDistance = FloorHeight7;
					OnPassFloor7.Fire( activator );
					break;
				case 8:
					floorDistance = FloorHeight8;
					OnPassFloor8.Fire( activator );
					break;
				default:
					Log.Error( "Should not happen in elevator!" );
					break;
			}

			MoveDistance = floorDistance + Math.Abs( FloorHeight1 );
			MoveDir = new Angles( -90, 0, 0 );
			var dir_world = Transform.NormalToWorld( MoveDir.Forward ); // MoveDir.Direction
			PositionB = PositionA + dir_world * MoveDistance;
			
			if ( DebugFlags.HasFlag( EntityDebugFlags.Text ) )
			{
				DebugOverlay.Text( $"State: {FloorHeight1}\nProgress: {floorDistance}", WorldSpaceBounds.Center, 10, Color.White );
			
				//var dir_world = MoveDir.Forward;
				//if ( MoveDirIsLocal ) dir_world = Transform.NormalToWorld( MoveDir.Forward );
			
				DebugOverlay.Line( Position, Position + dir_world * 100 );
			}

			_ = MoveElevator( floor, floorDistance, activator );
		}

		[Input]
		public void Lock()
		{
			Locked = true;
		}

		[Input]
		public void Unlock()
		{
			Locked = false;
		}

		[Input]
		public void Open()
		{

		}

		[Input]
		public void Close()
		{

		}
		#endregion

		Vector3 PositionA;
		Vector3 PositionB;

		public override void Spawn()
		{
			base.Spawn();

			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );

			PositionA = LocalPosition;
			PositionB = PositionA + MoveDir.Forward * MoveDistance; // MoveDir.Direction

			MoveDirIsLocal = true;

			IsMoving = false;
			IsMovingForwards = true;

			if ( MoveDirIsLocal )
			{
				var dir_world = Transform.NormalToWorld( MoveDir.Forward ); // MoveDir.Direction
				PositionB = PositionA + dir_world * MoveDistance;
			}
		}

		public bool OnUse( Entity user )
		{
			return false;
		}

		public bool IsUsable( Entity user )
		{
			//TODO: implement lock checks
			return true;
		}

		int movement = 0;
		async Task MoveElevator( int floor, float floorDistance, Entity activator = null )
		{
			// TODO: implement movement sound
			//if ( !IsMoving )
			//{
			//	Sound.FromEntity( StartMoveSound, this );
			//}

			IsMoving = true;
			var moveId = ++movement;

			var position = IsMovingForwards ? PositionB : PositionA;

			var distance = Vector3.DistanceBetween( LocalPosition, position );
			var timeToTake = distance / Math.Max( Speed, 0.1f );

			var success = await LocalKeyframeTo( position, timeToTake, null );
			if ( !success )
				return;

			switch ( floor )
			{
				case 1:
					_ = OnReachFloor1.Fire( activator );
					break;
				case 2:
					_ = OnReachFloor2.Fire( activator );
					break;
				case 3:
					_ = OnReachFloor3.Fire( activator );
					break;
				case 4:
					_ = OnReachFloor4.Fire( activator );
					break;
				case 5:
					_ = OnReachFloor5.Fire( activator );
					break;
				case 6:
					_ = OnReachFloor6.Fire( activator );
					break;
				case 7:
					_ = OnReachFloor7.Fire( activator );
					break;
				case 8:
					_ = OnReachFloor8.Fire( activator );
					break;
				default:
					Log.Error( "This should not happen!" );
					break;
			}

			if ( moveId != movement || !this.IsValid() ) return;

			if ( !IsMoving ) return;

			movement++;
			_ = LocalKeyframeTo( LocalPosition, 0, null ); // Bad
			_ = LocalRotateKeyframeTo( LocalRotation, 0, null );

			IsMoving = false;
		}
	}
}
