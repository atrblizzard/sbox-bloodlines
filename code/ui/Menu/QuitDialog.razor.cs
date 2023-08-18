using Sandbox;
using Sandbox.UI;

namespace Bloodlines.Menu;

/// <summary>
/// Popup dialog box asking the player if they want to quit i.e. MessagePanel.cs
/// </summary>
public partial class QuitDialog : MenuOverlay
{
	public QuitDialog()
	{
		SetClass("open", true);
	}
	public void OnClickCancel()
	{
		Close();
	}

	public void OnClickQuit()
	{
		Sandbox.Game.Menu.LeaveServer( "Disconnect" );
		Close();
	}

	public override void Close()
	{
		SetClass("open", false);
		base.Close();
	}
}
