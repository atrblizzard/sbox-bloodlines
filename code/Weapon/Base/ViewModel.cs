using Amper.FPS;
using Sandbox;

namespace Vampire;

public partial class ViewModel : SDKViewModel
{
	private AnimatedEntity Attachment;

	public override void PlaceViewmodel()
	{
		base.PlaceViewmodel();
		Camera.Main.SetViewModelCamera( 75 );
	}

	public AnimatedEntity GetAttachment() => Attachment;

	public override void SetWeaponModel( string viewmodel, SDKWeapon entity )
	{
		ClearWeapon( entity );

		var weapon = entity as WeaponBase;
		if ( !weapon.IsValid() )
			return;

		Weapon = weapon;

		var handsModel = GetPlayerHandsModel();
		SetModel( viewmodel );
		Attachment = CreateAttachment( handsModel );

		EnableShadowCasting = true;
		Attachment.EnableShadowCasting = true;
	}

	public string GetPlayerHandsModel()
	{
		var player = Player as VampirePlayer;
		if ( !player.IsValid() )
			return string.Empty;

		return player.PlayerClan.Hands;
	}

	public override bool ShouldDraw()
	{
		var player = Player as VampirePlayer;
		if ( !player.IsValid() )
			return false;

		var activeWeapon = player.ActiveWeapon;
		if ( (activeWeapon as SniperRifle)?.IsZoomed ?? false ) 
			return false;

		return true;
	}

	public override void CalculateView()
	{
		if ( Client.IsListenServerHost && sv_debug_viewmodel) //&& Game.IsServer )
		{
			DebugOverlay.ScreenText( CreateDebugTestString(), new Vector2( 560, 150 ) );
		}
		CalculateViewBob( );
		AddViewModelBobHelper();
	}
	
	private string CreateDebugTestString()
	{
		var player = Player as VampirePlayer;
		var str =
			$"[ViewModel]\n" +
			$"Name                  {Name}\n" +
			$"Model Name            {GetModelName()}\n" +
			$"Active Weapon:         {player?.ActiveWeapon}\n" +
			$"Tags:                 {string.Join(",", Tags.List)}\n" +
			$"Has AnimGraph:        {HasAnimGraph()}\n" +
			$"Has Hands Attachment: {GetAttachment()}\n" +
			$"\n";

		return str;
	}
	
	[ConVar.Replicated] public static bool sv_debug_viewmodel { get; set; }
}