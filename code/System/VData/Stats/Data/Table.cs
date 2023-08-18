using System.Collections.Generic;

namespace Vampire.System.Stats;

public struct Table
{
    public string Name { get; set; }
    public string InternalName { get; set; }
    public int Clamping { get; set; }
	public string TraitDependency { get; set; }
	public Dictionary<string, string> KeyValuePairs { get; set; }
}

// ActiveDisciplines