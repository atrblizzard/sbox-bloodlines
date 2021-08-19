using Sandbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.entities.core.Func
{
	[Library( "func_door_rotating" )]
	[Hammer.Solid]
	[Hammer.RenderFields]
	public partial class DoorRotating : KeyframeEntity, IUse
	{
		[Flags]
		public enum Flags
		{
			UseOpens = 1,
			StartLocked = 2,
			//SpawnOpen = 4,
			//OneWay = 8,
			//Touch = 16,

			//StartUnbreakable = 524288,
		}

		/// <summary>
		/// Settings that are only applicable when the entity spawns
		/// </summary>
		[Property( "spawnsettings", Title = "Spawn Settings", FGDType = "flags" )]
		public Flags SpawnSettings { get; set; } = Flags.UseOpens;

		/// <summary>
		/// The direction the door will move, when it opens.
		/// </summary>
		[Property( "movedir", Title = "Move Direction (Pitch Yaw Roll)" )]
		//[Internal.DefaultValue( "0 0 0" )]
		public Angles MoveDir { get; set; }

		/// <summary>
		/// Moving door: The amount, in inches, of the door to leave sticking out of the wall it recedes into when pressed. Negative values make the door recede even further into the wall.
		/// Rotating door: The amount, in degrees, that the door should rotate when it's pressed.
		/// </summary>
		[Property( "distance" )]
		public float Distance { get; set; } = 0;

		/// <summary>
		/// How far the door should be open on spawn where 0% = closed and 100% = fully open.
		/// </summary>
		[Property( "initial_position", Group = "Spawn Settings" )]
		[Hammer.MinMax( 0, 100 )]
		public float InitialPosition { get; set; } = 0;

		/// <summary>
		/// The speed at which the door moves.
		/// </summary>
		[Property( "speed" )]
		public float Speed { get; set; } = 100;

		/// <summary>
		/// Amount of time, in seconds, after the door has opened before it closes automatically. If the value is set to -1, the door never closes itself.
		/// </summary>
		[Property( "close_delay", Title = "Auto Close Delay (-1 stay)" )]
		public float TimeBeforeReset { get; set; } = 4;

		/// <summary>
		/// If set, opening this door will open all doors with given entity name. You can also simply name all doors the same for this to work.
		/// </summary>
		[Property( "other_doors_to_open", FGDType = "target_destination" )]
		public string OtherDoorsToOpen { get; set; } = "";

		/// <summary>
		/// Sound to play when the door starts to open.
		/// </summary>
		[Property( "open_sound", Group = "Sounds", FGDType = "sound", Title = "Start Opening Sound" )]
		public string OpenSound { get; set; } = "";

		/// <summary>
		/// Sound to play when the door reaches it's fully open position.
		/// </summary>
		[Property( "fully_open_sound", Group = "Sounds", FGDType = "sound", Title = "Fully Open Sound" )]
		public string FullyOpenSound { get; set; } = "";

		/// <summary>
		/// Sound to play when the door starts to close.
		/// </summary>
		[Property( "close_sound", Group = "Sounds", FGDType = "sound", Title = "Start Closing Sound" )]
		public string CloseSound { get; set; } = "";

		/// <summary>
		/// Sound to play when the door reaches it's fully closed position.
		/// </summary>
		[Property( "fully_closed_sound", Group = "Sounds", FGDType = "sound", Title = "Fully Closed Sound" )]
		public string FullyClosedSound { get; set; } = "";

		/// <summary>
		/// Sound to play when the door is attempted to be opened, but is locked.
		/// </summary>
		[Property( "locked_sound", Group = "Sounds", FGDType = "sound", Title = "Locked Sound" )]
		public string LockedSound { get; set; } = "";

		/// <summary>
		/// Sound to play while the door is moving. Typically this should be looping or very long.
		/// </summary>
		[Property( "moving_sound", Group = "Sounds", FGDType = "sound", Title = "Moving Sound" )]
		public string MovingSound { get; set; } = "";

		/// <summary>
		/// If checked, rotating doors will try to open away from the activator
		/// </summary>
		[Property( "open_away", Title = "Open Away From Player", Group = "Spawn Settings"  )]
		public bool OpenAwayFromPlayer { get; set; } = false;

		public bool Locked;

		Vector3 PositionA;
		Vector3 PositionB;
		Rotation RotationA;
		Rotation RotationB;
		Rotation RotationB_Normal;
		Rotation RotationB_Opposite;

		/// <summary>
		/// The easing function for both movement and rotation
		/// TODO: Expose to hammer in a nice way
		/// </summary>
		public Easing.Function Ease { get; set; } = Easing.EaseOut;

		public enum DoorState
		{
			Open,
			Closed,
			Opening,
			Closing
		}
		public DoorState State { get; protected set; } = DoorState.Open;

		public override void Spawn()
		{
			base.Spawn();

			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );

			RotationA = LocalRotation;

			var axis = Rotation.From( MoveDir ).Up;

			axis = Transform.NormalToLocal( axis );

			RotationB_Opposite = RotationA.RotateAroundAxis( axis, -Distance );
			RotationB_Normal = RotationA.RotateAroundAxis( axis, Distance );
			RotationB = RotationB_Normal;

			State = DoorState.Closed;
			Locked = SpawnSettings.HasFlag( Flags.StartLocked );

			if ( InitialPosition > 0 )
			{
				LocalRotation = Rotation.Lerp( RotationA, RotationB, InitialPosition / 100.0f ); 
				
				if ( InitialPosition >= 100.0f ) State = DoorState.Open;
			}

			if ( GetModel().HasData( "door_sounds" ) )
			{
				ModelDoorSounds sounds = GetModel().GetData<ModelDoorSounds>( "door_sounds" );

				if ( string.IsNullOrEmpty( MovingSound ) ) MovingSound = sounds.MovingSound;
				if ( string.IsNullOrEmpty( CloseSound ) ) CloseSound = sounds.CloseSound;
				if ( string.IsNullOrEmpty( FullyClosedSound ) ) FullyClosedSound = sounds.FullyClosedSound;
				if ( string.IsNullOrEmpty( OpenSound ) ) OpenSound = sounds.OpenSound;
				if ( string.IsNullOrEmpty( FullyOpenSound ) ) FullyOpenSound = sounds.FullyOpenSound;
				if ( string.IsNullOrEmpty( LockedSound ) ) LockedSound = sounds.LockedSound;
			}
		}

		protected override void OnDestroy()
		{
			if ( MoveSoundInstance.HasValue )
			{
				MoveSoundInstance.Value.Stop();
				MoveSoundInstance = null;
			}

			base.OnDestroy();
		}

		public virtual bool IsUsable( Entity user ) => SpawnSettings.HasFlag( Flags.UseOpens );

		/// <summary>
		/// Fired when a player tries to open/close this door with +use, but it's locked
		/// </summary>
		protected Output OnLockedUse { get; set; }

		public virtual bool Use( Entity user )
		{
			if ( Locked )
			{
				PlaySound( LockedSound );
				SetAnimBool( "locked", true );
				OnLockedUse.Fire( this );
				return false;
			}

			if ( SpawnSettings.HasFlag( Flags.UseOpens ) )
			{
				Toggle( user );
			}

			return false;
		}

		/// <summary>
		/// A player has pressed this
		/// </summary>
		public virtual bool OnUse( Entity user )
		{
			return Use( user );
		}

		public override void TakeDamage( DamageInfo info )
		{
			base.TakeDamage( info );
		}

		/// <summary>
		/// Toggle the open state of the door. Obeys locked state.
		/// </summary>
		[Input]
		public void Toggle( Entity activator = null )
		{
			if ( State == DoorState.Open || State == DoorState.Opening ) Close( activator );
			else if ( State == DoorState.Closed || State == DoorState.Closing ) Open( activator );
		}

		/// <summary>
		/// Open the door. Obeys locked state.
		/// </summary>
		[Input]
		public void Open( Entity activator = null )
		{
			if ( Locked )
			{
				PlaySound( LockedSound );
				SetAnimBool( "locked", true );
				return;
			}

			if ( State == DoorState.Closed )
			{
				PlaySound( OpenSound );
			}

			if ( State == DoorState.Closed || State == DoorState.Closing ) State = DoorState.Opening;

			if ( activator != null && OpenAwayFromPlayer )
			{
				var Pos1 = Position + RotationB_Normal.Forward * 10.0f;
				var Pos2 = Position + RotationB_Opposite.Forward * 10.0f;

				if ( activator.Position.Distance( Pos2 ) < activator.Position.Distance( Pos1 ) )
				{
					RotationB = RotationB_Normal;
				}
				else
				{
					RotationB = RotationB_Opposite;
				}
			}

			UpdateState();

			OpenOtherDoors( true, activator );
		}

		/// <summary>
		/// Close the door. Obeys locked state.
		/// </summary>
		[Input]
		public void Close( Entity activator = null )
		{
			if ( Locked )
			{
				PlaySound( LockedSound );

				SetAnimBool( "locked", true );
				return;
			}

			if ( State == DoorState.Open )
			{
				PlaySound( CloseSound );
			}

			if ( State == DoorState.Open || State == DoorState.Opening ) State = DoorState.Closing;

			UpdateState();

			OpenOtherDoors( false, activator );
		}

		/// <summary>
		/// Locks the door so it cannot be opened or closed.
		/// </summary>
		[Input]
		public void Lock()
		{
			Locked = true;
		}

		/// <summary>
		/// Unlocks the door.
		/// </summary>
		[Input]
		public void Unlock()
		{
			Locked = false;
		}

		/// <summary>
		/// Fired when the door starts to open. This can be called multiple times during a single "door opening"
		/// </summary>
		protected Output OnOpen { get; set; }

		/// <summary>
		/// Fired when the door starts to close. This can be called multiple times during a single "door closing"
		/// </summary>
		protected Output OnClose { get; set; }

		/// <summary>
		/// Called when the door fully opens.
		/// </summary>
		protected Output OnFullyOpen { get; set; }

		/// <summary>
		/// Called when the door fully closes.
		/// </summary>
		protected Output OnFullyClosed { get; set; }

		public bool ShouldPropagateState = true;
		void OpenOtherDoors( bool open, Entity activator )
		{
			if ( !ShouldPropagateState ) return;

			List<Entity> ents = new List<Entity>();

			if ( !string.IsNullOrEmpty( EntityName ) ) ents.AddRange( Entity.FindAllByName( EntityName ) );
			if ( !string.IsNullOrEmpty( OtherDoorsToOpen ) ) ents.AddRange( Entity.FindAllByName( OtherDoorsToOpen ) );

			foreach ( var ent in ents )
			{
				if ( ent == this || !(ent is DoorEntity) ) continue;

				DoorEntity door = (DoorEntity)ent;

				door.ShouldPropagateState = false;
				if ( open )
				{
					door.Open( activator );
				}
				else
				{
					door.Close( activator );
				}
				door.ShouldPropagateState = true;
			}
		}

		public virtual void UpdateState()
		{
			bool open = State == DoorState.Opening || State == DoorState.Open;

			_ = DoMove( open );
		}

		int movement;
		Sound? MoveSoundInstance;

		async Task DoMove( bool state )
		{
			if ( !MoveSoundInstance.HasValue && !string.IsNullOrEmpty( MovingSound ) )
			{
				MoveSoundInstance = PlaySound( MovingSound );
			}

			var moveId = ++movement;

			if ( State == DoorState.Opening )
			{
				_ = OnOpen.Fire( this );
			}
			else if ( State == DoorState.Closing )
			{
				_ = OnClose.Fire( this );
			}

			var target = state ? RotationB : RotationA;

			Rotation diff = LocalRotation * target.Inverse;
			var timeToTake = diff.Angle() / Math.Max( Speed, 0.1f );

			var success = await LocalRotateKeyframeTo( target, timeToTake, Ease );
			if ( !success )
				return;

			if ( moveId != movement || !this.IsValid() )
					return;

			if ( State == DoorState.Opening )
			{
				_ = OnFullyOpen.Fire( this );
				State = DoorState.Open;
				PlaySound( FullyOpenSound );
			}
			else if ( State == DoorState.Closing )
			{
				_ = OnFullyClosed.Fire( this );
				State = DoorState.Closed;
				PlaySound( FullyClosedSound );
			}

			if ( MoveSoundInstance.HasValue )
			{
				MoveSoundInstance.Value.Stop();
				MoveSoundInstance = null;
			}

			if ( state && TimeBeforeReset >= 0 )
			{
				await Task.DelaySeconds( TimeBeforeReset );

				if ( moveId != movement || !this.IsValid() )
					return;

				Toggle();
			}
		}
	}
}
