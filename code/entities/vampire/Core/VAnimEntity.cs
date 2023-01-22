using Sandbox;

namespace bloodlines.entities.vampire
{
	public class VAnimEntity : AnimatedEntity
	{
		[Property("starthidden", Title = "Start Hidden")]
		public int StartHidden { get; set; }

		[Input]
		protected void ScriptHide(Entity activator)
		{
			Hide(activator);
		}

		[Input]
		protected void ScriptUnhide(Entity activator)
		{
			EnableAllCollisions = true;
			EnableDrawing = true;
			//CollisionGroup = CollisionGroup.Always;
		}

		public void Hide(Entity activator = null)
		{
			EnableDrawing = false;
			EnableAllCollisions = false;
			//CollisionGroup = CollisionGroup.Never;
		}

		public override void Spawn()
		{
			base.Spawn();

			if (StartHidden == 1)
				Hide();
		}
	}
}


