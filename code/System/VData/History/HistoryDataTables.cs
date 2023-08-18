using Sandbox;
using System.Collections.Generic;
using Vampire.System.VData.History.Data;

[GameResource("History Data", "history", "VTM:B History Definitions")]
public class HistoryDataTables : GameResource
{
	public List<HistoryData> HistoryData { get; set; }
}
