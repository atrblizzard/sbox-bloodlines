#define WORLDEVENTS

using Sandbox;
using System;
using System.Linq;
using System.Threading.Tasks;
using Bloodlines.Game.System.Dialog;
using Bloodlines.UI;
using MyGame;

[Library( "vampiremp", Title = "Vampire Multiplayer Games" )]
public partial class VampireMPGame : GameManager
{
	public static VampireMPGame Entity => Current as VampireMPGame;
	public int StoryState { get; set; }	
	public Hud Hud { get; set; }
	
	

    public VampireMPGame()
    {
        if (Game.IsServer)
        {
            // TODO: Add future server only functoins
        }

        if (Game.IsClient)
        {
            Game.RootPanel = new Hud();
        }
    }

    public override void ClientSpawn()
    {
	    Game.RootPanel?.Delete(true);
	    Game.RootPanel = new Hud();
	    
	    base.ClientSpawn();
    }

    public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var player = new VampirePlayer();

        if (client.IsUsingVr)
        {
	        client.Pawn = new VRPawn();
        }
        else
        {
	        client.Pawn = player;
        }

        player.UniqueRandomSeed = Game.Random.Int( 0, 999999 );
        player.Respawn();

        // Get all of the spawnpoints
        var spawnpoints = All.OfType<SpawnPoint>();

        // chose a random one
        var randomSpawnPoint = spawnpoints.MinBy(_ => Guid.NewGuid());

        // if it exists, place the pawn there
        if (randomSpawnPoint != null)
        {
            player.Transform = randomSpawnPoint.Transform;

            if (client.IsUsingVr)
            {
	            var tx = randomSpawnPoint.Transform;
	            tx.Position += Vector3.Up * 50.0f; // raise it up
                var vrPawn = (VRPawn)client.Pawn;
                vrPawn.Transform = tx;
            }
        }
    }
    
    public override void ClientDisconnect( IClient client, NetworkDisconnectionReason reason )
    {
	    Log.Info($"Client {client.Name} Disconnected");
	    base.ClientDisconnect( client, reason );
    }

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

    public void DoPlayerNoclip( IClient player )
	{
		if ( player.Pawn is Player basePlayer )
		{
			if ( basePlayer.DevController is NoclipController )
			{
				Log.Info( "Noclip Mode Off" );
				basePlayer.DevController = null;
			}
			else
			{
				Log.Info( "Noclip Mode On" );
				basePlayer.DevController = new NoclipController();
			}
		}
	}

	public override void PostLevelLoaded()
	{
		Log.Info( "PostLevelLoaded initializing!" );

#if WORLDEVENTS
        // foreach ( var worldspawn in All.OfType<WorldSpawn>() )
        // {
        // 	worldspawn.Test();
        // }
        //
        // Log.Info( Entity.All.OfType<WorldSpawn>().Count() );
        // Log.Info( Entity.All.OfType<WorldEntity>().Count() );
        // Log.Info( WorldEntity.All.First().SpawnFlags );
#endif

		base.PostLevelLoaded();
	}
}