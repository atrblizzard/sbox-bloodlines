using System.Collections.Generic;

namespace Vampire.System.VData.Items.Data;

public struct ItemTypes
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public List<ItemType> ItemType { get; set; }
}
