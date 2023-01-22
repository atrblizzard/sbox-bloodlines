using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Editor;
using Sandbox;

namespace bloodlines.entities.core.Func
{
	[Library( "prop_button", Description = "Prop Button" )]
	[HammerEntity, SupportsSolid]
	[RenderFields, VisGroup( VisGroup.Dynamic )]
	[Model( Archetypes = ModelArchetype.animated_model | ModelArchetype.static_prop_model )]
	[Title( "Button" ), Category( "Gameplay" ), Icon( "radio_button_checked" )]
	public partial class ButtonProp : KeyframeEntity, IUse
	{
		
		TimeSince LastUsed;
		public bool Momentary { get; set; } = false;
		
		/// <summary>
		/// Whether the button is locked or not.
		/// </summary>
		[Property]
		public bool Locked { get; set; }
		
		[Flags]
		public enum ActivationFlags
		{
			UseActivates = 1,
			DamageActivates = 2
		}
		
		/// <summary>
		/// How this button can be activated
		/// </summary>
		[Property]
		public ActivationFlags ActivationSettings { get; set; } = ActivationFlags.UseActivates;
		
		/// <summary>
		/// Sound played when the button is pressed and is unlocked
		/// </summary>
		[Property( "unlocked_sound", Title = "Activation Sound"), FGDType( "sound" )]
		public string UnlockedSound { get; set; }

		/// <summary>
		/// Sound played when the button is pressed and is locked
		/// </summary>
		[Property( "locked_sound", Title = "Locked Activation Sound"), FGDType( "sound" )]
		public string LockedSound { get; set; }
		
		[Property( "current_state", Title = "Current State")]
		public int CurrentState { get; set; }
		
		[Property( "max_states", Title = "Max State")]
		public int MaxStates { get; set; }
		
		[Property]
		public float Wait { get; set; }
		
		[Property]
		public float Speed { get; set; }

		// Fired when button goes to selected state 
		#region Outputs
		protected Output OnSetState1 { get; set; }
		protected Output OnSetState2 { get; set; }
		protected Output OnSetState3 { get; set; }
		protected Output OnSetState4 { get; set; }
		protected Output OnSetState5 { get; set; }
		protected Output OnSetState6 { get; set; }
		protected Output OnSetState7 { get; set; }
		protected Output OnSetState8 { get; set; }
		protected Output OnPressedLocked { get; set; }
		protected Output OnPressed { get; set; }
		protected Output OnReleased { get; set; }
		#endregion

		#region Inputs
        [Input]
        public void Lock( )
        {
	        Locked = true;
        }
        
        [Input]
        public void Unlock( )
        {
	        Locked = false;
        }
            
        [Input]
        protected void Press( Entity activator )
        {
	        DoPress( activator );
        }
                    
        [Input]
        protected void ToggleLock( )
        {
	        Locked = !Locked;
        }
                            
        [Input]
        protected void SetState(int state, Entity activator = null)
        {
	        Log.Info( "Setting state: " + state );
			switch ( state )
			{
				case 0:
					OnSetState1.Fire( activator );
					break;
				case 1:
					OnSetState2.Fire( activator );
					break;
				case 2:
					OnSetState3.Fire( activator );
					break;
				case 3:
					OnSetState4.Fire( activator );
					break;
				case 4:
					OnSetState5.Fire( activator );
					break;
				case 5:
					OnSetState6.Fire( activator );
					break;
				case 6:
					OnSetState7.Fire( activator );
					break;
				case 7:
					OnSetState8.Fire( activator );
					break;
				default:
					Log.Error( $"SetState on prop_button does not exist! {state}" );
					break;
			}
		}
		#endregion

		public override void Spawn()
        {
	        base.Spawn();

			SetModel( GetModelName() );

	        SetupPhysicsFromModel( PhysicsMotionType.Static );
	        
	        EnableSolidCollisions = true;
	        
	        if ( SetupPhysicsFromModel( PhysicsMotionType.Keyframed ) == null )
	        {
		        Log.Warning( $"{this} has a model {Model} with no physics!" );
	        }

	        // if ( SpawnSettings.HasFlag( Flags.NonSolid ) )
	        // {
		       //  EnableSolidCollisions = false;
	        // }
        }
		
		public void Pressed( Entity toucher )
		{
			OnPressed.Fire( toucher );
		}

		public void LockedPressed( Entity toucher )
		{
			OnPressedLocked.Fire( toucher );
		}
		
		int globalTimers = 0;
		async Task FireRelease()
		{
			var thisTimer = ++globalTimers;

			await Task.DelaySeconds( 0.1f );

			if ( thisTimer != globalTimers ) return;

			_ = OnReleased.Fire( this );
		}

		
		public bool OnUse( Entity user )
		{
			if ( !ActivationSettings.HasFlag( ActivationFlags.UseActivates ) ) return false;
			
			if ( Locked )
			{
				//Sound.FromEntity( LockedSound, this );
				OnPressedLocked.Fire( user );
				Log.Info( "Locked shit" );
				return false;
			}
			
			if ( LastUsed > 0.1f ) 
				DoPress( user );

			LastUsed = 0;
			
			//DoPress( user );
			_ = FireRelease();

			return true;
		}

		private void DoPress( Entity activator )
        {
	        if ( Locked )
	        {
		        PlaySound( LockedSound );
		        return;
	        }

			PlaySound( UnlockedSound );
			OnPressed.Fire( activator );
        }

		public bool IsUsable( Entity user )
		{
			// TODO: implement lock usability
			//return true; //!Locked;
			
			var hasFlag = ActivationSettings.HasFlag( ActivationFlags.UseActivates );
			bool shit = hasFlag && !Locked;
			
			Log.Info( "Is shit usable: " + shit );
			return hasFlag && !Locked;
		}
	}
}
