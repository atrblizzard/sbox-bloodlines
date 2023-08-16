﻿using Editor;
using Sandbox;

namespace Bloodlines.Entities.Vampire;

[Library( "item_container_lock", Description = "Container Lock" )]
[Model]
[HammerEntity]
public class ItemContainerLock : VAnimEntity, IUse
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
