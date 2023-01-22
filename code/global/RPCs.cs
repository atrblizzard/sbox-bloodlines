using Sandbox;

namespace Bloodlines.Globals
{
	public partial class RPCs
    {
		[ClientRpc]
		public static void OpenSignPanel( string signData )
		{
			if ( !string.IsNullOrEmpty( signData ) )
				Event.Run( "vampire.signpanel.open", signData );			
		}
	}
}
