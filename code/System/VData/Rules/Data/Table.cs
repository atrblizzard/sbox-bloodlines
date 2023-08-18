using System.Collections.Generic;

public struct Table
{
	public string Name { get; set; }
	public string InternalName { get; set; }
	public int Clamping { get; set; }
    public string TraitDependency { get; set; }

    // Feat value / Vision distance (in game units)
    public Dictionary<string, int> KeyValuePairs { get; set; }
}