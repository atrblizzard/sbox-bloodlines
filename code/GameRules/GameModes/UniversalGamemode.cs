using Sandbox;

namespace Vampire
{
	public abstract class UniversalGamemode : BaseNetworkable, IGamemode
	{
		public virtual string Title => ToString();
		public virtual string Icon => IGamemode.DEFAULT_ICON;

		public virtual GamemodeProperties Properties => default;
		public virtual int Priority => 0;

		public abstract bool HasWon(out Team team, out WinReason reason);

		public abstract bool IsActive();

		public virtual bool ShouldSwapTeams(Team winner, WinReason winReason)
		{
			return Properties.SwapTeamsOnRoundRestart;
		}
	}
}
