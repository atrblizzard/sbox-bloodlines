
using Bloodlines.UI.HUD;
using bloodlines.ui;
using bloodlines.ui.Vampire;
using Sandbox;
using Sandbox.UI;

[Library]
public partial class DeathmatchHud : HudEntity<RootPanel>
{
	public static DeathmatchHud? Current { get; private set; }

	private readonly Toast _toast;

	public DeathmatchHud()
    {
        if (!IsClient)
            return;

		RootPanel.StyleSheet.Load("swb_base/deathmatch_dep/ui/scss/DeathmatchHud.scss");

		//RootPanel.AddChild<Vitals>();
		//RootPanel.AddChild<Ammo>();

		Current = this;

		//RootPanel.AddChild<Inspector>();

		RootPanel.AddChild<HealthBarHud>();
		RootPanel.AddChild<BloodBarHud>();

		RootPanel.AddChild<NameTags>();
        RootPanel.AddChild<DamageIndicator>();
				
        RootPanel.AddChild<InventoryBar>();
        RootPanel.AddChild<PickupFeed>();

        RootPanel.AddChild<ChatBox>();
        RootPanel.AddChild<KillFeed>();
        RootPanel.AddChild<Scoreboard>();
        RootPanel.AddChild<VoiceList>();

		RootPanel.AddChild<DialogPanel>();
		RootPanel.AddChild<SignPanel>();

		RootPanel.AddChild( out _toast, "toast-lower" );

#if SANDBOX_ENABLED
		RootPanel.AddChild<CurrentTool>();
		RootPanel.AddChild<SpawnMenu>();
#endif
	}

	[Event.Hotload]
	public static void OnHotReloaded()
	{
		if ( Host.IsClient )
		{
			Local.Hud?.Delete();

			DeathmatchHud hud = new();

			//if ( Local.Client.Pawn is TTTPlayer player && player.LifeState == LifeState.Alive )
			//{
			//	hud.AliveHudPanel.Enabled = true;
			//}
		}
	}

	[ClientRpc]
    public void OnPlayerDied(string victim, string attacker = null)
    {
        Host.AssertClient();
    }

    [ClientRpc]
    public void ShowDeathScreen(string attackerName)
    {
        Host.AssertClient();
    }

	public void Toast( string message )
	{
		if ( !IsClient ) return;
		_toast.Show( message );
	}
}
