using Sandbox;
using Editor;

namespace bloodlines.entities.core.Func
{
	[Library( "func_door" )]
	[Model]
	[HammerEntity]
	[SupportsSolid]
	[DoorHelper( "movedir", "movedir_islocal", "movedir_type", "lip" )]
	[RenderFields]
	public partial class Door : DoorEntity
	{
		public override void Spawn()
		{
			MoveDirType = DoorMoveType.Moving;
			base.Spawn();
		}
	}
}
