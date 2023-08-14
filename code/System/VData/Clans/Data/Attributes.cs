using System.Collections.Generic;

namespace Vampire.System.Clans;

public struct Attributes
{
	public List<Dictionary<string, string>> Text { get; set; }
	
	public string AttributeOrder { get; set; }
	
	public BaseAttributes Base { get; set; }
	public int Wits { get; set; }
	public int Intelligence { get; set; }
	public int Perception { get; set; }
	public int Appearance { get; set; }
	public int Manipulation { get; set; }
	public int Charisma { get; set; }
	public int Stamina { get; set; }
	public int Dexterity { get; set; }
	public int Strength { get; set; }
	public int Experience { get; set; }
	
	public int MaxHealth { get; set; }
	public int BloodPool { get; set; }
	public int FaithPoints { get; set; }
	
	public string StartingEquipment { get; set; }
	public string ExcludedEquipment { get; set; }
}