using Sandbox;

namespace bloodlines.entities.vampire.Events
{
	//[Library( "worldspawn" )]
	public partial class WorldSpawn : WorldEntity
	{
		[Property( "safearea" )]
		public int SafeArea { get; set; } = 0;

		[Property( "nofrenzyarea" )]
		public int NoFrenzyArea { get; set; } = 0;

		[Property( "nosferatu_tolerrant" )]
		public int NosferatuTolerrant { get; set; } = 0;

		[Property( "copwaitarea" )]
		public int CopWaitArea { get; set; } = 0;

		[Property( "comment" )]
		public string Comment { get; set; }

		public override void Spawn()
		{
			Log.Info( "Loading worldspawn" );
			Log.Info( Comment );
			base.Spawn();
		}
	}
}
