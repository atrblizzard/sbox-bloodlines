using Amper.FPS;
using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Vampire
{
	partial class VampireGameRules
	{
		[ConVar.Replicated] public static bool v_balanceteams { get; set; } = true;
		[ConVar.Replicated] public static bool v_swapteamsonroundend { get; set; } = true;

		[Net] public IDictionary<Team, TeamRole> TeamRole { get; set; }
		[Net] public IDictionary<Team, string> TeamGoal { get; set; }

		public override void DeclareGameTeams()
		{
			base.DeclareGameTeams();

			TeamManager.DeclareTeam((int)Team.Vampire, "vampire", "Vampires", new Color(0xB8383B));
			TeamManager.DeclareTeam((int)Team.Hunter, "hunter", "Vampire Hunters", new Color(0x5885A2));
		}

		public override void OnTeamLose(int team)
		{
			base.OnTeamLose(team);

			var loser = (Team)team;
			foreach (var ply in loser.GetPlayers())
			{
				//ply.AddCondition(TFCondition.Humiliated);
			}
		}

		public override void PlayTeamWinSong(int team)
		{
			PlaySoundToTeam((int)(Team)team, "announcer.your_team.won", SoundBroadcastChannel.Announcer);
		}

		public override void PlayTeamLoseSong(int team)
		{
			PlaySoundToTeam((int)(Team)team, "announcer.your_team.lost", SoundBroadcastChannel.Announcer);
		}

		/// <summary>
		/// Swap players on Vampires or Hunters to opposite team
		/// </summary>
		protected void SwapAllPlayersTeam()
		{
			var players = All.OfType<VampirePlayer>().ToList();
			foreach (var player in players)
			{
				var current = player.Team;
				if (current is Team.Vampire or Team.Hunter)
				{
					player.TeamNumber = current == Team.Vampire ? (int)Team.Hunter : (int)Team.Vampire;
				}
			}
		}

		public override bool CanChangeTeamTo(int newTeam)
		{
			var team = (Team)newTeam;
			var players = All.OfType<VampirePlayer>();
			var enumerable = players as VampirePlayer[] ?? players.ToArray();
			var teamCount = enumerable.Where(x => x.Team == team).Count();
			var enemyCount = enumerable.Where(x => x.Team == team.GetEnemy()).Count();

			if (v_balanceteams && teamCount > enemyCount) return false;

			return base.CanChangeTeamTo(newTeam);
		}
	}
}