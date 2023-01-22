using Sandbox;
using Editor;

namespace bloodlines.entities.core.Func
{
	[Library( "func_door_rotating" )]
	[Model]
	[HammerEntity]
	[SupportsSolid]
	[DoorHelper( "movedir", "movedir_islocal", "movedir_type", "lip" )]
	[RenderFields]
	public partial class DoorRotating : DoorEntity
	{
		public override void Spawn()
		{
			MoveDirType = DoorMoveType.Rotating;
			base.Spawn();
		}
	}
}
