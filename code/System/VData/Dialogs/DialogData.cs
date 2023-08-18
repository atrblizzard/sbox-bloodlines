using System.Collections.Generic;
using Sandbox;

namespace Vampire.Data.Dialog;

[GameResource("Dialog", "dialog", "VTM:B Dialog Definitions")]
public class DialogData : GameResource
{
    public List<NPCDialogEntry> Entries { get; set; }
}