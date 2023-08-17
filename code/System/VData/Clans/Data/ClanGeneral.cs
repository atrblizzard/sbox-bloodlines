using System.Collections.Generic;
using Vampire.Data.Dialog;

namespace Vampire.Data.Clans;

public struct ClanGeneral
{
	public string Clan { get; set; }
	public string Body { get; set; }
	public string M_Body { get; set; }
	public string F_Body { get; set; }
	public Dictionary<Gender, string> Gender { get; set; }
	public List<string> History { get; set; }
	public bool Kindred { get; set; }
	public bool Supernatural { get; set; }
	public bool Boss { get; set; }
	public string ClanIcon { get; set; }
	public ClanEffects ClanEffect { get; set; }
	public bool IsBlueBlood { get; set; }
	public bool HasBlueBlood { get; set; }
}