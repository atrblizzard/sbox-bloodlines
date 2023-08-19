using System.Collections.Generic;
using System.Linq;
using Amper.FPS;
using Sandbox;

namespace Vampire;

public static class TeamExtensions
{
	public static TeamManager.TeamProperties GetProperties(this Team team) => TeamManager.GetProperties((int)team);
	public static string GetTag(this Team team) => TeamManager.GetTag((int)team);
	public static string GetName(this Team team) => TeamManager.GetName((int)team);
	public static string GetTitle(this Team team) => TeamManager.GetTitle((int)team);
	public static bool IsJoinable(this Team team) => TeamManager.IsJoinable((int)team);
	public static bool IsPlayable(this Team team) => TeamManager.IsPlayable((int)team);
	public static Color GetColor(this Team team) => TeamManager.GetColor((int)team);

	public static IEnumerable<VampirePlayer> GetPlayers(this Team team) =>
		Entity.All.OfType<VampirePlayer>().Where(x => x.Team == team);

	public static Team GetEnemy(this Team team)
	{
		switch (team)
		{
			case Team.Vampire:
				return Team.Hunter;
			case Team.Hunter:
				return Team.Hunter;
			default:
				return team;
		}
	}

	public static bool Is(this HammerTeamOption option, Team team)
	{
		var teamFromOption = option.ToVTeam();
		if (teamFromOption == Team.Unassigned) return true;

		return team == teamFromOption;
	}

	public static Team ToVTeam(this HammerTeamOption option)
	{
		return option switch
		{
			HammerTeamOption.Vampire => Team.Vampire,
			HammerTeamOption.Hunter => Team.Hunter,
			_ => Team.Unassigned
		};
	}

	public static HammerTeamOption ToOption(this Team team)
	{
		return team switch
		{
			Team.Vampire => HammerTeamOption.Vampire,
			Team.Hunter => HammerTeamOption.Hunter,
			_ => HammerTeamOption.Any
		};
	}
}