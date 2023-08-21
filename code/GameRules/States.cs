using System.Linq;
using Sandbox;

namespace Vampire;

public partial class VampireGameRules
{
	[ConVar.Replicated] public static bool mp_tournament_readymode { get; set; } = false;
	public override bool WaitingForPlayersEnabled() => !ReadyUpEnabled() && !mp_waiting_for_players_cancel;
	public override bool ReadyUpEnabled() => mp_tournament_readymode || GetGamemode()?.Properties.RequireReadyUp == true;
	public override void OnRoundRestart()
	{
		FirstBloodAnnounced = false;

		//foreach ( var ply in Entity.All.OfType<VampirePlayer>() )
		//	ply.ResetPoints();

		// foreach ( var building in Entity.All.OfType<TFBuilding>().ToArray() )
		// 	building.ManualDestroy();
	}

	int LastAnnouncerSeconds { get; set; }
	bool WillPlayGameStartSong { get; set; }
	[Net] public bool HasSetup { get; set; }
	
	[Net] public bool IsInSetup { get; set; }
}