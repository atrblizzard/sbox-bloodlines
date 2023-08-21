namespace Vampire;

/// <summary>
/// If you are adding custom gamemode, inherit from <see cref="MapGamemode"/> or <see cref="UniversalGamemode"/> instead!
/// </summary>
public interface IGamemode
{
	public const string DEFAULT_ICON = "ui/icons/empty.png";

	public string Title { get; }
	public string Icon { get; }
	public GamemodeProperties Properties { get; }
	/// <summary>
	/// When the IsActive check for this gamemode should happen relative to other gamemodes
	/// </summary>
	public int Priority { get; }
	public bool IsActive();
	public bool HasWon( out Team team, out WinReason reason );
	public bool ShouldSwapTeams(Team winner, WinReason winReason);
}
