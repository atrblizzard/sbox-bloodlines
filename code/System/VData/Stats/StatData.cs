using Sandbox;
namespace Vampire.System.Stats;

[GameResource("Stats Data", "stats", "VTM:B Stats Definitions")]
public class StatData : GameResource
{
	public MappingTraits Attributes { get; set; }
	public MappingTraits Abilities { get; set; }
	public MappingTraits Disciplines { get; set; }
}

// ActiveDisciplines