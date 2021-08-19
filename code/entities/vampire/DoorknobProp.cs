using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bloodlines.entities.core.Func;

namespace bloodlines.entities.vampire
{
	[Hammer.Model]	
	[Hammer.RenderFields]
	[Library( "prop_doorknob", Description = "Generic Doorknob" )]
	public partial class DoorknobProp : KeyframeEntity, IUse
	{
		[Property("use_icon", Title = "Use Icon")]
		public int UseIcon { get; set; }
		
		[Property("difficulty", Title = "Difficulty", Hammer = true)]
		public int Difficulty { get; set; }
		
		[Property( Title = "Model Name" )]
		public string Model { get; set; }

		[Property( Title = "Parent Name" )]
		public string ParentName { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			SetModel( Model );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		public bool IsUsable( Entity user )
		{
			// Return true for now
			// TODO: Add skill based checks to be able to use doorknob
			return true;
		}

		public virtual bool OnUse( Entity user )
		{
			List<Entity> ents = new List<Entity>();

			if ( !string.IsNullOrEmpty( EntityName ) ) ents.AddRange( Entity.FindAllByName( EntityName ) );
			if ( !string.IsNullOrEmpty( ParentName ) ) ents.AddRange( Entity.FindAllByName( ParentName ) );

			foreach ( var ent in ents )
			{
				if ( ent == this || !(ent is DoorRotating) ) continue;
				DoorRotating door = (DoorRotating)ent;

				door.Use( user );
			}
			return false;
		}
	}
}
