using bloodlines.entities.vampire.NPC;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sandbox.ButtonEntity;

namespace bloodlines.entities.vampire
{
	[Library( "prop_switch", Description = "Prop Switch" )]
	[Hammer.Model]
	public partial class Switch : VAnimEntity, IUse
	{
		[Flags]
		public enum Flags
		{
			UseActivates = 1,
			DamageActivates = 2,
			//TouchActivates = 4,
			//Toggle = 8,
		}

		[FGDType("flags")]
		[Property( "spawnsettings", Title = "Spawn Settings")]
		public Flags SpawnSettings { get; set; } = Flags.UseActivates;

		[Property( Title = "Start Disabled" )]
		public bool StartDisabled { get; set; } = false;

		[Property( Title = "Activate Sound" )]
		public string Actsnd { get; set; }

		[Property( Title = "Locked Sound" )]
		public string Locksnd { get; set; }

		[Property( Title = "Deactivate Sound" )]
		public string Deactsnd { get; set; }

		public enum SwitchState
		{
			On,
			Off,
			Activating,
			Deactivating
		}
		public SwitchState State { get; protected set; } = SwitchState.On;

		bool AnimGraphFinished = false;

		public override void Spawn()
		{
			base.Spawn();

			if (!string.IsNullOrEmpty(GetModelName()))
				SetModel( GetModel() );

			SetupPhysicsFromModel( PhysicsMotionType.Static );
		}

		protected Output OnLockedUse { get; set; }
		protected Output OnActivate { get; set; }
		protected Output OnDeactivate { get; set; }

		public virtual void Activate( Entity toucher )
		{
			OnActivate.Fire( toucher );
		}

		public virtual void Deactivate( Entity toucher )
		{
			OnDeactivate.Fire( toucher );
		}

		/// <summary>
		/// A player has pressed this
		/// </summary>
		public virtual bool OnUse( Entity user )
		{
			Toggle( user );

			return false;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}

		[Input]
		public void Toggle( Entity activator = null )
		{
			switch (State)
			{
				case SwitchState.On:
					Close( activator );
					break;
				case SwitchState.Off:
					Open( activator );
					break;
			}
		}

		[Input]
		public void Open( Entity activator = null )
		{
			if ( State == SwitchState.Off || State == SwitchState.Deactivating )
				State = SwitchState.Activating;

			UpdateAnimGraph( true );

			UpdateState();
		}

		[Input]
		public void Close( Entity activator = null )
		{
			if ( State == SwitchState.On || State == SwitchState.Activating )
				State = SwitchState.Deactivating;

			UpdateAnimGraph( false );
			UpdateState();
		}

		public virtual void UpdateState()
		{
			bool open = (State == SwitchState.Activating) || (State == SwitchState.On);

			_ = DoMove( open );
		}

		void UpdateAnimGraph( bool open )
		{		
			SetAnimBool( "activated", open );
		}

		protected override void OnAnimGraphCreated()
		{
			base.OnAnimGraphCreated();

			UpdateAnimGraph( true );
		}

		async Task DoMove( bool state )
		{
			if ( State == SwitchState.Activating )
			{
				PlaySound( "small_metal_switch.on" );

				await Task.Delay( 2000 );
				_ = OnActivate.Fire( this );
				State = SwitchState.On;
	
			}
			else if ( State == SwitchState.Deactivating)
			{
				PlaySound( "small_metal_switch.off" );
				await Task.Delay( 900 );
				State = SwitchState.Off;
				_ = OnDeactivate.Fire( this );
			}			
		}

		protected override void OnAnimGraphTag( string tag, AnimGraphTagEvent fireMode )
		{
			if ( tag == "AnimationFinished" && fireMode != AnimGraphTagEvent.End )
			{
				AnimGraphFinished = true;
			}
		}
	}
}
