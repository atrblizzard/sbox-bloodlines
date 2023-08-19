using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Amper.FPS;
using Bloodlines.UI;
using Amper.FPS;
using Sandbox;

namespace Vampire
{
    [Display(Name = "Vampire the Masquerade: Bloodlines")]
    public partial class VampireGameRules : SDKGame
    {
        [ConVar.Replicated] public static bool v_enable_devcam { get; set; } = true;
        [ConVar.Replicated] public static VampirePlayerClan v_bot_force_class { get; set; } = VampirePlayerClan.Tremere;
        
        public new static VampireGameRules Current { get; set; }

        public VampireGameRules()
        {
            Current = this;
            Movement = new VampireGameMovement();
            
            if ( Game.IsServer )
            {
                _ = new VampireHud();
            }
        }

        public override void Tick()
        {
            base.Tick();

            //if (Game.IsServer)
            //    TickRespawnWaves();

            foreach (Team team in Enum.GetValues(typeof(Team)))
            {
                RespawnTeam(team);
            }
        }
        
        public void RespawnTeam( Team team, bool force = false )
        {
            RespawnPlayers( force, true, (int)team );
        }

        public override void ClientJoined(IClient client)
        {
            var player = new VampirePlayer();
            player.Respawn();
            client.Pawn = player;

            ShowServerMessage(To.Single(client));

            // send this to everyone's chat
            //TFChatBox.AddInformation(To.Everyone, $"{client.Name} has joined the game");
            Log.Info($"{client.Name} has joined the game!");

            // Force player to default clan and team until the UI is implemented            
            player.PlayerClan = PlayerClan.Get( v_bot_force_class );
            player.ChangeTeam( player.GetAutoTeam(), true, false );

            // Get all of the spawnpoints
            var spawnPoints = Entity.All.OfType<SpawnPoint>();

            // chose a random one
            var randomSpawnPoint = spawnPoints.MinBy(x => Guid.NewGuid());

            // if it exists, place the pawn there
            if (randomSpawnPoint != null)
            {
                var tx = randomSpawnPoint.Transform;
                tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
                player.Transform = tx;
            }
        }

        public override void ClientDisconnect(IClient client, NetworkDisconnectionReason reason)
        {
            base.ClientDisconnect(client, reason);

            // send this to everyone's chat
            Log.Info($"{client.Name} has left the game!");
            // TFChatBox.AddInformation(To.Everyone, $"{client.Name} has left the game");
        }

        public virtual void PlayerChangeClass(VampirePlayer player, PlayerClan pclass)
        {
            if (!Game.IsServer)
                return;

            // Call a game event.
            EventDispatcher.InvokeEvent(new PlayerChangeClassEvent
            {
                Client = player.Client,
                Class = pclass
            });
        }

        // [ClientRpc] public void ShowTeamSelectionMenu() { HudOverlay.Open<TeamSelection>(); }
        // [ClientRpc] public void ShowClassSelectionMenu() { HudOverlay.Open<ClassSelection>(); }
        [ClientRpc]
        public void ShowServerMessage()
        {
            Log.Info("Here we show the server message!");
            /*HudOverlay.Open<ServerMessage>();*/
            
        }

        public virtual void PlayerRegenerate(VampirePlayer player, bool full)
        {
            if (!Game.IsServer)
                return;

            // Call a game event.
            //EventDispatcher.InvokeEvent(new PlayerRegenerateEvent
            //{
            //    Client = player.Client
            //});
        }
        
        public override Trace SetupSpawnTrace( SDKPlayer player, Vector3 from, Vector3 to, Vector3 mins, Vector3 maxs )
        {
            return base.SetupSpawnTrace( player, from, to, mins, maxs )
                .WithoutTags( "team_barrier" );
        }
        
        public override void DoPlayerDevCam( IClient client )
        {
            if ( v_enable_devcam )
                base.DoPlayerDevCam( client );
        }
    }
}

public static class VCollisionTags
{
    public const string TeamBarrier = "team_barrier";
}