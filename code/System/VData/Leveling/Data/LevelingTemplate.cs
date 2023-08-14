using System.Collections.Generic;

namespace Vampire.System.VData.Leveling.Data;

public struct LevelingTemplate
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public string ClanDependency { get; set; }

    public List<LevelGroup> LevelGroup { get; set; }
}