using Sandbox;

namespace bloodlines.entities.core.Func
{
	[Library( "prop_button", Description = "Prop Button" )]
	[Hammer.Model]
	public partial class ButtonProp : ModelEntity, IUse
	{
		public bool Locked { get; set; }

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
		#endregion

		#region Inputs
        [Input]
        public void Lock( )
        {
        			
        }
        
        [Input]
        public void Unlock( )
        {
                	
        }
            
        [Input]
        public void Press( )
        {
                    
        }
                    
        [Input]
        public void ToggleLock( )
        {
                    
        }
                            
        [Input]
        public void SetState(int state, Entity activator = null)
        {
			switch ( state )
			{
				case 1:
					OnSetState1.Fire( activator );
					break;
				case 2:
					OnSetState2.Fire( activator );
					break;
				case 3:
					OnSetState3.Fire( activator );
					break;
				case 4:
					OnSetState4.Fire( activator );
					break;
				case 5:
					OnSetState5.Fire( activator );
					break;
				case 6:
					OnSetState6.Fire( activator );
					break;
				case 7:
					OnSetState7.Fire( activator );
					break;
				case 8:
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

			SetModel( GetModel() );

	        SetupPhysicsFromModel( PhysicsMotionType.Static );
        }
		
		public virtual void Pressed( Entity toucher )
		{
			OnPressed.Fire( toucher );
		}

		public virtual void LockedPressed( Entity toucher )
		{
			OnPressedLocked.Fire( toucher );
		}
		
		public bool OnUse( Entity user )
		{
			DoPress( user );
			return false;
		}
		
		internal void DoPress( Entity activator )
        {
        	if ( Locked )
        	{
				//Sound.FromEntity( LockedSound, this );
				OnPressedLocked.Fire( activator );
        	}

			// TODO: check where OnSetState outputs should be fired
			OnPressed.Fire( activator );
			OnSetState1.Fire( activator );
			OnSetState2.Fire( activator );
        }

		public bool IsUsable( Entity user )
		{
			// TODO: implement lock usability
			return true;
		}
	}
}
