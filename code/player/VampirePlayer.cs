using System;
using Amper.FPS;
using Sandbox;
using System.Linq;
using Vampire.System.VData.Weapons;

//using Vampire.UI;

namespace Vampire
{
    public partial class VampirePlayer : SDKPlayer
    {
        public new static VampirePlayer LocalPlayer => Game.LocalPawn as VampirePlayer;
        public override float DeathAnimationTime => 2;

        public override void Spawn()
        {
		    base.Spawn();
            
            CurrentSequence.Name = "tremere_female_idle2";
        } 
        
        public override void Respawn()
        {
            //We wish to change our class to something else.
            if (DesiredPlayerClan.IsValid())
            {
                CurrentPlayerClan = DesiredPlayerClan;
                DesiredPlayerClan = null;
            }
            
            base.Respawn();
    
            // We are respawning and we have a class selected.
            if (CurrentPlayerClan.IsValid())
            {
                Tags.Add(CurrentPlayerClan.GetTag());
            
                SetupPlayerClan();
                Regenerate(true);
            }
        }

        public override void Touch(Entity other)
        {
            var player = other as VampirePlayer;

            //if (player != null)
            //    CheckTouchingSpies(player);

            base.Touch(other);
        }

        public override bool IsReadyToPlay()
        {
            if (!PlayerClan.IsValid())
                 return false;

            return base.IsReadyToPlay();
        }

        public override void Tick()
        {
            base.Tick();

            if (!IsAlive)
                return;
        }
        
        public float GetClassEyeHeight()
        {
            if ( PlayerClan == null )
                return base.GetPlayerViewOffset( false ).z;

            return PlayerClan.EyeHeight;
        }
        
        public override Vector3 GetPlayerViewOffset( bool ducked )
        {
            var vec = base.GetPlayerViewOffset( ducked );

            if ( !ducked )
                vec = vec.WithZ( GetClassEyeHeight() );

            return vec;
        }
        
        public override ViewVectors ViewVectors => new()
        {
            ViewOffset = new( 0, 0, 72 ),

            HullMin = new( -16, -16, 0 ),
            HullMax = new( 16, 16, 72 ),

            DuckHullMin = new( -16, -16, 0 ),
            DuckHullMax = new( 16, 16, 62 ),
            DuckViewOffset = new( 0, 0, 45 ),

            ObserverHullMin = new( -10, -10, -10 ),
            ObserverHullMax = new( 10, 10, 10 ),

            DeadViewOffset = new( 0, 0, 14 )
        };
        
        public override SDKViewModel CreateViewModel() => new ViewModel();
        public WeaponSlot GetActiveTFSlot() => (WeaponSlot)GetActiveSlot();
        public WeaponBase GetWeaponInSlot( WeaponSlot slot ) => GetWeaponInSlot( (int)slot ) as WeaponBase;
        
        public override float CalculateMaxSpeed()
        {
            if ( !CurrentPlayerClan.IsValid() )
                return 0;

            var maxSpeed = CurrentPlayerClan.MaxSpeed;
            
            if (Input.Down("run"))
                maxSpeed = CurrentPlayerClan.MaxSpeed * 1.5f;
            if ( maxSpeed <= 0 )
                Velocity = 0;

            return maxSpeed;
        }

        public bool CanRegenerate()
        {
            return true;
        }

        public void Regenerate(bool full = false)
        {
            if (!Game.IsServer)
                return;
            
            // The player wanted to change class but wasnt able to, respawn them now.
            if ( DesiredPlayerClan.IsValid() )
            {
                Respawn();
                return;
            }
            
            if ( full )
            {
                // If we do full regeneration, delete our entire inventory.
                DeleteAllWeapons();
            }
            
            // Reset health to max health.
            if ( Health < GetMaxHealth() )
                Health = GetMaxHealth();
            
            RegenerateWeaponsForClan( PlayerClan );
            
            VampireGameRules.Current.PlayerRegenerate( this, full );
        }

        public override void Simulate(IClient cl)
        {
            base.Simulate(cl);

            if (!IsAlive)
                return;

            SimulateCameraLogic();

            ShowTestDebugOverlay();
        }

        [GameEvent.Client.BuildInput]
        void MenuInputs()
        {
            // if (Input.Pressed("Team"))
            // {
            //     if (HudOverlay.CurrentMenu is TeamSelection)
            //         HudOverlay.CloseActive();
            //     else
            //         HudOverlay.Open<TeamSelection>();
            // }
            // else if (Input.Pressed("Class"))
            // {
            //     if (HudOverlay.CurrentMenu is ClassSelection)
            //         HudOverlay.CloseActive();
            //     else
            //         HudOverlay.Open<ClassSelection>();
            // }
        }

        [Net] public PlayerClan PlayerClan { get; set; }

        public Team Team => (Team)TeamNumber;

        public bool IsEnemy(VampirePlayer player) => Team != player.Team;
        public bool IsAlly(VampirePlayer player) => Team == player.Team;

        public Team GetAutoTeam( Team preferredTeam = Team.Unassigned )
        {
            var players = All.OfType<VampirePlayer>();
            var vPlayers = players.ToList();
            var vampirePlayersCount = vPlayers.Where( x => x.Team == Team.Vampire ).Count();
            var hunterPlayersCount = vPlayers.Where( x => x.Team == Team.Hunter ).Count();

            // hunter has less players than vampire.
            if ( hunterPlayersCount < vampirePlayersCount )
                return Team.Hunter;

            // vampire has less players than hunter.
            if ( vampirePlayersCount < hunterPlayersCount )
                return Team.Vampire;

            // AutoTeam should give new players to the attackers on A/D maps if the teams are even
            //if ( VampireGameRules.Current.IsAttackDefense() )
            //return VTeam.Hunter;

            // we don't have a preferred team, pick a random one.
            if ( preferredTeam == Team.Unassigned )
                return Game.Random.Int( 0, 1 ) == 1 ? Team.Vampire : Team.Hunter;

            // return our preference.
            return preferredTeam;
        }
        
        [ConCmd.Server( "join_team" )]
        public static void Command_SetTeam( Team team )
        {
            if ( ConsoleSystem.Caller.Pawn is not VampirePlayer player ) 
                return;

            var autoTeamed = false;
            
            team = player.GetAutoTeam();

            // Actually change the team.
            if ( !player.ChangeTeam( team, autoTeamed, false ) )
                return;

            // If player joined a playable team, show them class selection menu.
            if ( team.IsPlayable() )
            {
                player.Respawn();
            }
        }
        
        public bool ChangeTeam( Team team, bool autoTeam, bool silent, bool autoBalance = false )
        {
            bool result = ChangeTeam( (int)team, autoTeam, silent, autoBalance );
            Log.Info($"ChangeTeam Result: {result}");

            return result;
        }
        
        private void ShowTestDebugOverlay()
        {
            if ( Client.IsListenServerHost && sv_debug_player )
            {
                DebugOverlay.ScreenText( CreateDebugTestString(), new Vector2( 60, 150 ) );
            }
        }
        
        public void RegenerateWeaponsForClan( PlayerClan pclass )
        {
            var weaponsList = ResourceLibrary.GetAll<WeaponData>().ToList();

            foreach (var weaponData in weaponsList)
            {
                // No weapon here
                if (weaponData == null)
                    return;

                if (!weaponData.IsValid())
                    return;

                var newWeapon = weaponData.CreateInstance();
                EquipWeapon(newWeapon);
                
                if ( !ActiveWeapon.IsValid() )
                    SwitchToNextBestWeapon();
            }
        }

        [ConCmd.Server("give_weapon")]
        private static void Cmd_GiveWeapon(string weaponName)
        {
            if (ConsoleSystem.Caller.Pawn is not VampirePlayer localPlayer)
                return;
            
            var data = ResourceLibrary.GetAll<WeaponData>().FirstOrDefault( x => x.EngineClass == weaponName);
            if (data != null)
            {
                if (!data.IsValid())
                    return;
                
                var weapon = data.CreateInstance();
                if (weapon != null)
                {
                    Log.Info(weapon.Name);
                    weapon.Regenerate();
                    localPlayer.EquipWeapon(weapon);
                    localPlayer.SwitchToWeapon(weapon);
                }
            }
        }

        [ConCmd.Server("weapon_switch")]
        private static void Cmd_SwitchToWeapon(string weaponName)
        {
            if (ConsoleSystem.Caller.Pawn is not VampirePlayer localPlayer)
                return;
            
            foreach (var weapon in localPlayer.Weapons)
            {
                if (weapon.ClassName == weaponName)
                    localPlayer.ForceSwitchWeapon(weapon);
            }
        }
        
        protected override bool PreEquipWeapon( SDKWeapon weapon, bool makeActive )
        {
            // This is overriden from base because each class has weapon in a different slot.
            var newWeapon = weapon as WeaponBase;
            if ( !newWeapon.IsValid() )
                return base.PreEquipWeapon( weapon, makeActive );

            //Can't be equipped by this class.
            // if ( !newWeapon.Data.TryGetOwnerDataForPlayerClan( PlayerClan, out var ownerData ) )
            //     return false;

            if ( !CanDrop( weapon ) )
                return false;

            return true;
        }

        [ConVar.Replicated] public static bool sv_debug_player { get; set; }

        private string CreateDebugTestString()
        {
            var str =
                $"[PLAYER]\n" +
                $"Name                  {Name}\n" +
                $"Eye Height            {GetClassEyeHeight()}\n" +
                $"GroundEntity          {GroundEntity}\n" +
                $"MoveType              {MoveType}\n" +
                $"Flags                 {Flags}\n" +
                $"Clan                  {PlayerClan}\n" +
                $"Team                  {TeamNumber}\n" +
                $"ObserverMode          {ObserverMode}\n" +
                $"HoveredEntity         {HoveredEntity}\n" +
                $"Last Known Area:      #{(LastKnownArea?.ID.ToString()) ?? "~"}\n" +
                $"Tags:                 {string.Join(",", Tags.List)}\n" +
                $"\n";

            return str;
        }
    }
}
