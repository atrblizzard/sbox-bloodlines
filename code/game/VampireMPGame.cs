#define WORLDEVENTS

using Sandbox;
using Sandbox.Pawns;
using System;
using System.Linq;
using System.Threading.Tasks;
using Bloodlines.Game.System.Dialog;
using Bloodlines.UI;
using MyGame;
//using Vampire.UI;

[Library( "vampiremp", Title = "Vampire Multiplayer Games" )]
partial class VampireMPGame : GameManager
{
	public int StoryState { get; set; }	
	public DialogManager DialogManager { get; set; }

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

    public override void ClientJoined( IClient cl )
	{
		base.ClientJoined( cl );

		var player = new VampirePlayer();

        if (cl.IsUsingVr)
        {
            cl.Pawn = new VRPawn();
        }
        else
        {
            cl.Pawn = player;
        }

        player.Respawn();

        // Get all of the spawnpoints
        var spawnpoints = All.OfType<SpawnPoint>();

        // chose a random one
        var randomSpawnPoint = spawnpoints.MinBy(_ => Guid.NewGuid());

        // if it exists, place the pawn there
        if (randomSpawnPoint != null)
        {
            player.Transform = randomSpawnPoint.Transform;

            if (cl.IsUsingVr)
            {
	            var tx = randomSpawnPoint.Transform;
	            tx.Position += Vector3.Up * 50.0f; // raise it up
                var vrPawn = (VRPawn)cl.Pawn;
                vrPawn.Transform = tx;
            }
        }
    }

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

    [ConCmd.Server("spawn")]
    public static async Task Spawn(string modelname)
    {
        var owner = ConsoleSystem.Caller?.Pawn as Player;

        if (ConsoleSystem.Caller == null)
            return;

        var tr = Trace.Ray(owner.EyePosition, owner.EyePosition + owner.EyeRotation.Forward * 500)
            .UseHitboxes()
            .Ignore(owner)
            .Run();

        var modelRotation = Rotation.From(new Angles(0, owner.EyeRotation.Angles().yaw, 0)) * Rotation.FromAxis(Vector3.Up, 180);

        var model = Model.Load(modelname);
        if (model == null || model.IsError)
            return;

        var ent = new Prop
        {
            Position = tr.EndPosition + Vector3.Down * model.PhysicsBounds.Mins.z,
            Rotation = modelRotation,
            Model = model
        };

        // Let's make sure physics are ready to go instead of waiting
        ent.SetupPhysicsFromModel(PhysicsMotionType.Dynamic);

        // If there's no physics model, create a simple OBB
        if (!ent.PhysicsBody.IsValid())
        {
            ent.SetupPhysicsFromOBB(PhysicsMotionType.Dynamic, ent.CollisionBounds.Mins, ent.CollisionBounds.Maxs);
        }
    }


    [ConCmd.Server("spawn_entity")]
    public static void SpawnEntity(string entName)
    {
        var owner = ConsoleSystem.Caller.Pawn as Player;

        if (owner == null)
            return;

        var entityType = TypeLibrary.GetType<Entity>(entName)?.TargetType;
        if (entityType == null)
            return;

        if (!TypeLibrary.HasAttribute<SpawnableAttribute>(entityType))
            return;

        var tr = Trace.Ray(owner.EyePosition, owner.EyePosition + owner.EyeRotation.Forward * 200)
            .UseHitboxes()
            .Ignore(owner)
            .Size(2)
            .Run();

        var ent = TypeLibrary.Create<Entity>(entityType);
        if (ent is BaseCarriable && owner.Inventory != null)
        {
            if (owner.Inventory.Add(ent, true))
                return;
        }

        ent.Position = tr.EndPosition;
        ent.Rotation = Rotation.From(new Angles(0, owner.EyeRotation.Angles().yaw, 0));

        Log.Info($"ent: {ent}");
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

        DialogManager = new DialogManager();

		base.PostLevelLoaded();
	}
}