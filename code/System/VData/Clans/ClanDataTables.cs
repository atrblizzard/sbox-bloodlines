using System.Collections.Generic;
using Sandbox;

namespace Vampire.Data.Clans;

[GameResource("Clan Data tables", "clans", "VTM:B Clan Table Definitions")]
public class ClanDataTables : GameResource
{
	public List<ClanData> ClanData { get; private set; } = new();

	public ClanData GetClan(int clanIndex)
	{
		return ClanData[clanIndex];
	}

	public int GetClanIndex(ClanData clan)
	{
		return ClanData.FindIndex(x => x == clan);
	}
}