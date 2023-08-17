using System.Collections.Generic;

namespace Vampire.Data.Clans;

public struct Abilities
{
	public string AbilityOrder { get; set; }
	public int Athletics { get; set; }
	public int Brawl { get; set; }
	public int Dodge { get; set; }
	public int Intimidation { get; set; }
	public int Subterfuge { get; set; }
	public int AnimalKen { get; set; }
	public int Firearms { get; set; }
	public int Melee { get; set; }
	public int Security { get; set; }
	public int Stealth { get; set; }
	public int Computer { get; set; }
	public int Finance { get; set; }
	public int Investigation { get; set; }
	public int Academics { get; set; }
	public int Occult { get; set; }

	public Dictionary<string, int> Disciplines { get; set; }
}