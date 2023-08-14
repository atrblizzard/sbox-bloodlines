using System.Collections.Generic;
using Sandbox;

namespace Bloodlines.Game.System.Dialog;

[GameResource("Dialog", "dialog", "VTM:B Dialog Definitions")]
public class DialogData : GameResource
{
    public List<NPCDialogEntry> Entries { get; set; }
}