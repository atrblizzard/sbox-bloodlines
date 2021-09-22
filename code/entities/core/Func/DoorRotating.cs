﻿using Sandbox;

namespace bloodlines.entities.core.Func
{
	[Library( "func_door_rotating" )]
	[Hammer.Model]
	[Hammer.SupportsSolid]
	[Hammer.DoorHelper( "movedir", "movedir_islocal", "movedir_type", "lip" )]
	[Hammer.RenderFields]
	public partial class DoorRotating : DoorEntity
	{
		public override void Spawn()
		{
			MoveDirType = DoorMoveType.Rotating;
			base.Spawn();
		}		
	}
}
