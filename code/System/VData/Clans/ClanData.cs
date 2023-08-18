using Sandbox;

namespace Vampire.Data.Clans;

[GameResource("Clan Data", "clan", "VTM:B Clan Definitions")]
public class ClanData : GameResource
{
	public ClanText Text { get; set; }
	public ClanGeneral General { get; set; }
	public Attributes Attributes { get; set; }
	public Disciplines Disciplines { get; set; }
}