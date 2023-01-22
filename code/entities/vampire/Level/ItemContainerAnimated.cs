using Editor;
using Sandbox;

namespace bloodlines.entities.vampire;

[Library( "item_container_animated", Description = "Animated Container" )]
[Model]
[HammerEntity]
public class ItemContainerAnimated : VAnimEntity, IUse
{
	public virtual bool OnUse( Entity user )
	{
		Toggle( user );

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
	
	[Input]
	public void Toggle( Entity activator = null )
	{
		
	}
}
