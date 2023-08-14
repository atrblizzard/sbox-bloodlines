using System.Collections.Generic;

namespace Bloodlines.Game.System.Dialog;

public class NPCDialogEntry : IDialogEntry
{
	public int Id { get; set; }
	public Dictionary<Gender, string> Text { get; set; }
	public string Condition { get; set; }
	public string Action { get; set; }
	public int Link { get; set; }

	public List<DialogEntry> Responses { get; set; } = new();
}