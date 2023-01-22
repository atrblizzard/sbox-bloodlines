using bloodlines.entities.core.Func;
using Sandbox;
using System.Collections.Generic;
using Editor;

namespace bloodlines.entities.vampire
{
	[Model]
	[RenderFields]
	[HammerEntity]
	[Library( "prop_doorknob_electronic", Description = "Electronic Doorknob" )]
	public partial class ElectronicDoorknobProp : VAnimEntity, IUse
	{
		[Property( "use_icon", Title = "Use Icon" )]
		public int UseIcon { get; set; }

		[Property( "difficulty", Title = "Difficulty" )]
		public int Difficulty { get; set; }

		[HideInEditor]
		[Property( "parentname", Title = "Parent Name" )]
		public string ParentName { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			SetModel( GetModelName() );

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

			if ( !string.IsNullOrEmpty( Name ) ) ents.AddRange( FindAllByName( Name ) );
			if ( !string.IsNullOrEmpty( ParentName ) ) ents.AddRange( FindAllByName( ParentName ) );

			foreach ( var ent in ents )
			{
				if ( ent == this || !(ent is DoorRotating) ) continue;
				DoorRotating door = (DoorRotating)ent;

				door.OnUse( user );
			}
			return false;
		}
	}
}
