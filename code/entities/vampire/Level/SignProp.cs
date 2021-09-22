namespace bloodlines.entities.vampire.Game
{
	using bloodlines.entities.vampire.Data;
	using Sandbox;

	[Library( "prop_sign", Description = "Sign prop" )]
	[Hammer.Model]
	public partial class SignProp : ModelEntity, ISignData, IUse
	{
		public bool Locked { get; set; }

		public SignData SignData { get; set; }

		public bool IsUsable( Entity user )
		{
			throw new System.NotImplementedException();
		}

		public bool OnUse( Entity user )
		{
			return false;
		}

		public override void Spawn()
		{
			base.Spawn();

			SetModel( GetModel() );

			SetupPhysicsFromModel( PhysicsMotionType.Static );
		}

		public void InitializeHUD( Entity activator )
		{
			// TODO: Implement panel 
		}
	}
}
