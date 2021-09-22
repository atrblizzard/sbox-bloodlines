using Sandbox;

namespace bloodlines.entities.vampire
{
	public class VEntity : Entity
	{
		[Property( "starthidden", Title = "Start Hidden" )]
		public int StartHidden { get; set; }

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

			if ( StartHidden == 1 )
				Hide();
		}
	}
}
