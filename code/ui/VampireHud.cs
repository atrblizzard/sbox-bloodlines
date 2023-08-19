using Bloodlines.UI;
using Sandbox;

namespace Bloodlines.UI;

public partial class VampireHud : HudEntity<VampireRootPanel>
{
	[ConVar.Client( "cl_drawhud" )] public static bool Enabled { get; set; } = true;

	public static VampireHud Instance { get; set; }

	public VampireHud()
	{
		Instance = this;
	}

	[GameEvent.Client.Frame]
	public void OnHudChangeEnabled()
	{
		if ( Enabled )
		{
			if ( !RootPanel.IsVisible )
				RootPanel.Style.Set( "display", "flex" );
		}
		else
		{
			if ( RootPanel.IsVisible )
				RootPanel.Style.Set( "display", "none" );
		}
	}
}
