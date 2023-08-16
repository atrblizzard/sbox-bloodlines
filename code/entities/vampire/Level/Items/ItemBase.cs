using Bloodlines.Entities.Vampire;
using Editor;

namespace Sandbox.Entities.Vampire.Level.Items;

[Model]
public class ItemBase : VAnimEntity, IUse
{
	public bool OnUse( Entity user )
	{
		throw new System.NotImplementedException();
	}

	public bool IsUsable( Entity user )
	{
		throw new System.NotImplementedException();
	}
}
