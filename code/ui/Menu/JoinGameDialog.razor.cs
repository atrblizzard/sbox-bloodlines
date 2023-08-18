using Sandbox;
using Sandbox.UI;

namespace Bloodlines.Menu;

public partial class JoinGameDialog : MenuOverlay
{
	public string SearchText { get; set; } = "";

	public void OnClickCancel()
	{
		Close();
	}

	public void OnClickConnect()
	{
		if(ulong.TryParse(SearchText, out var steamid))
			Sandbox.Game.Menu.ConnectToServer( steamid );
	}
}
