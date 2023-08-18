using Sandbox.UI;

namespace Bloodlines.Menu;

public class MenuOverlay : Panel
{
	/// <summary>
	/// Current active menu overlay.
	/// </summary>
	public static MenuOverlay CurrentMenu { get; set; }

	public static bool IsActive => CurrentMenu != null;

	public static T Open<T>() where T : MenuOverlay, new()
	{
		var overlay = new T();
		Log.Info(typeof(T).Name);
		return Open( overlay ) as T;
	}

	public static MenuOverlay Open( MenuOverlay overlay )
	{
		Log.Info("Trying to open " + overlay.ElementName);
		CloseActive();

		MainMenu.Current?.FindRootPanel()?.AddChild( overlay );
		CurrentMenu = overlay;
        return CurrentMenu;
	}

	public static void CloseActive()
	{
		Log.Info("Trying to close active " + CurrentMenu?.ElementName);
		CurrentMenu?.Close();
	}

	public virtual void Close()
	{
		if ( CurrentMenu == this )
			CurrentMenu = null;
		
		Delete();
	}
}
