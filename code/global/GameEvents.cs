using bloodlines.entities.vampire.Data;
using Sandbox;

namespace Bloodlines.Globals
{
	public static class GameEvents
	{
		public const string PlayerDamaged = "vampire.player.damaged";
		public const string PlayerDisconnected = "vampire.player.disconnected";
		public const string DebugTick = "vampire.debug.tick";
		public const string ScreenEffect = "vampire.screenfx";

		public static class SignPanel
		{
			private const string Prefix = "vampire.signpanel.";

			public const string Toggle = Prefix + "toggle";
			public const string Close = Prefix + "close";
			public const string Open = Prefix + "open";
			public const string UpdatedFile = Prefix + "updatedfile";
			public const string Cleared = Prefix + "cleared";

			public class ToggleAttribute : EventAttribute
			{
				public ToggleAttribute(SignData signData) : base( Toggle ) { }
			}

			public class OpenAttribute : EventAttribute
			{
				public OpenAttribute( SignData signData ) : base( Open ) { }
			}

			public class CloseAttribute : EventAttribute
			{
				public CloseAttribute() : base( Close ) { }
			}

			public class UpdatedFileAttribute : EventAttribute
			{
				public UpdatedFileAttribute() : base( UpdatedFile ) { }
			}

			public class ClearedAttribute : EventAttribute
			{
				public ClearedAttribute() : base( Cleared ) { }
			}
		}
	}
}
