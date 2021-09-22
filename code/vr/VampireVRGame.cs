
using Bloodlines.VR.Player;
using Sandbox;

/// <summary>
/// You don't need to put things in a namespace, but it doesn't hurt.
/// </summary>
namespace Bloodlines.VR.Game
{
	[Library( "vampirevr" )]
	public partial class VampireVRGame : Sandbox.Game
	{
		public override void PostCameraSetup( ref CameraSetup camSetup )
		{
			base.PostCameraSetup( ref camSetup );

			camSetup.ZNear = 1f;
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new VRPlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}
}
