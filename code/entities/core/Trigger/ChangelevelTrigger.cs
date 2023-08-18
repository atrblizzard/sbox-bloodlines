using Editor;
using Sandbox;

namespace Vampire.entities.core.Trigger;

[Solid]
[HammerEntity]
[Library("trigger_changelevel")]
public partial class ChangelevelTrigger : BaseTrigger
{
	[Property(Title = "Map")]
	public string Map { get; set; }
	
	[Property(Title = "Landmark")]
	public string Landmark { get; set; }
}