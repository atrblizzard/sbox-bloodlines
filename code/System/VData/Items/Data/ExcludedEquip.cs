using System.Collections.Generic;

namespace Vampire.System.VData.Items.Data;

public struct ExcludedEquip
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public Dictionary<ExcludeFlags, string> Flags { get; set; }
}