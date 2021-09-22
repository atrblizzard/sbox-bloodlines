using bloodlines.entities.vampire.Events;
using bloodlines.entities.vampire.Game;
using Sandbox;
using System.Linq;

[Library( "bloodlines", Title = "Vampire" )]
partial class VampireGame : Game
{
	public int StoryState { get; set; }

	public VampireGame()
	{
		if ( IsServer )
		{
			// Create the HUD
			_ = new DeathmatchHud();
		}
	}

	public override void PostCameraSetup( ref CameraSetup camSetup )
	{
		base.PostCameraSetup( ref camSetup );

		if ( VR.Enabled )
			camSetup.ZNear = 1f;

		camSetup.ZNear = 0.5f;
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );
		var player = new VampirePlayer();
		player.Respawn();

		cl.Pawn = player;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	[ServerCmd( "spawn" )]
	public static void Spawn( string modelname )
	{
		var owner = ConsoleSystem.Caller?.Pawn;

		if ( ConsoleSystem.Caller == null )
			return;

		var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 500 )
			.UseHitboxes()
			.Ignore( owner )
			.Run();

		var ent = new Prop
		{
			Position = tr.EndPos,
			Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) ) * Rotation.FromAxis( Vector3.Up, 180 )
		};
		ent.SetModel( modelname );
		ent.Position = tr.EndPos - Vector3.Up * ent.CollisionBounds.Mins.z;
	}

	[ServerCmd( "spawn_entity" )]
	public static void SpawnEntity( string entName )
	{
		var owner = ConsoleSystem.Caller.Pawn;

		if ( owner == null )
			return;

		var attribute = Library.GetAttribute( entName );

		if ( attribute == null || !attribute.Spawnable )
			return;

		var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 200 )
			.UseHitboxes()
			.Ignore( owner )
			.Size( 2 )
			.Run();

		var ent = Library.Create<Entity>( entName );
		if ( ent is BaseCarriable && owner.Inventory != null )
		{
			if ( owner.Inventory.Add( ent, true ) )
				return;
		}

		ent.Position = tr.EndPos;
		ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) );
	}

	public override void DoPlayerNoclip( Client player )
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
	
	[ClientCmd( "debug_write" )]
	public static void Write()
	{
		ConsoleSystem.Run( "quit" );
	}

#if WORLDEVENTS
	public override void PostLevelLoaded()
	{
		Log.Info( "PostLevelLoaded initializing!" );

		foreach ( var worldspawn in All.OfType<WorldSpawn>() )
		{
			worldspawn.Test();
		}

		Log.Info( Entity.All.OfType<WorldSpawn>().Count() );
		Log.Info( Entity.All.OfType<WorldEntity>().Count() );
		Log.Info( WorldEntity.All.First().SpawnFlags );

		base.PostLevelLoaded();
	}
#endif
}
