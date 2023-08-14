using System.Collections.Generic;

namespace Vampire.System.VData.Items.Data;

public struct StartingEquip
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public int Money { get; set; }

    public Items Items { get; set; }
    public Dictionary<string, string> Item { get; set; }
}