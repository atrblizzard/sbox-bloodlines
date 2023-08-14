using Vampire.System.Character.Data;
using Vampire.System.Stats;

namespace Vampire.System.Character.Data.Feats;

public struct Feat
{
	public string Name { get; set; }
	public string InternalName { get; set; }
	public string HelpText { get; set; }
	public int MaxValue { get; set; }
	// "Base#" are the Traits that the Feat is based upon
	public string Base0 { get; set; }
	public string Base1 { get; set; }
	// "Weighting" is the dice-roll table to use when rolling this feat
	public Weightening PCWeighting { get; set; }
	public Weightening NPCWeighting { get; set; }

	public string Display2nd0 { get; set; }

	public Table Table { get; set; }
}

public enum Weightening { Normal }