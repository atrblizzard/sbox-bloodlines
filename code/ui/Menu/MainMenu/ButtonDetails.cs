using System;

namespace Bloodlines.Menu;

public struct ButtonDetails
{
	public ButtonDetails(string title, string tooltip, Action onClicked, bool isDisabled = false, MenuType menuType = MenuType.Both)
	{
		Title = title;
		Tooltip = tooltip;
		OnClicked = onClicked;
		IsDisabled = isDisabled;
        Type = menuType;
	}

	public string Title { get; }
	public string Tooltip { get; }
    public MenuType Type { get; }
    public bool IsDisabled { get; }
	public Action OnClicked { get; }

	public enum MenuType
	{
		Both = 0,
        InGameOnly = 1,
        MainMenuOnly = 2		
	}
}