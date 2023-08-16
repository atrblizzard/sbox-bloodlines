using Sandbox;

namespace Bloodlines.Entities.Vampire
{
	public class VEntity : Entity
	{
		[Property( "starthidden", Title = "Start Hidden" )]
		public bool StartHidden { get; set; }
		
		[Property( "start_enabled", Title = "Start Enabled" )]
		public bool StartEnabled { get; set; }

		[Input]
		protected void ScriptHide( Entity activator )
		{
			Hide( activator );
		}

		[Input]
		protected void ScriptUnhide( Entity activator )
		{
			EnableDrawing = true;
		}

		public void Hide( Entity activator = null )
		{
			EnableDrawing = false;
		}

		public override void Spawn()
		{
			base.Spawn();

			if ( StartHidden )
				Hide();
		}
	}
}
