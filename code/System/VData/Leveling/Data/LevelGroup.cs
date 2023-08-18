using System.Collections.Generic;

namespace Vampire.System.VData.Leveling.Data;

public struct LevelGroup
{
    public string Dependency { get; set; }
    public Dictionary<string, int> Level { get; set; }
}