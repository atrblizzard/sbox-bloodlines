using Sandbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.entities.core.Func
{
	[Library( "func_door" )]
	[Hammer.Model]
	[Hammer.SupportsSolid]
	[Hammer.DoorHelper( "movedir", "movedir_islocal", "movedir_type", "lip" )]
	[Hammer.RenderFields]
	public partial class Door : DoorEntity
	{
		public override void Spawn()
		{
			MoveDirType = DoorMoveType.Moving;
			base.Spawn();
		}
	}
}
