using System.Collections.Generic;

namespace Vampire.System.VData.Items.Data;

public struct ItemType
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public bool IsWielded { get; set; }
    public bool IsWorn { get; set; }
    public bool IsWeapon { get; set; }
    public List<string> InventorySection { get; set; }
}