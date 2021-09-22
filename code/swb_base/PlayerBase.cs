using System.Linq;
using Sandbox;

namespace SWB_Base
{
    partial class PlayerBase : Player
    {
        TimeSince timeSinceDropped;

		[Net] public PawnController VehicleController { get; set; }
		[Net] public PawnAnimator VehicleAnimator { get; set; }
		[Net, Predicted] public ICamera VehicleCamera { get; set; }
		[Net, Predicted] public Entity Vehicle { get; set; }
		[Net, Predicted] public ICamera MainCamera { get; set; }

		public ICamera LastCamera { get; set; }

		public bool SupressPickupNotices { get; set; }

//        public PlayerBase()
//        {
//            Inventory = new DmInventory(this);
//        }

        public PlayerBase(IBaseInventory inventory = null)
        {
            if (inventory != null)
            {
                Inventory = inventory;
            }
            else
            {
                Inventory = new InventoryBase(this);
            }
        }

		public override void Spawn()
		{
			MainCamera = new FirstPersonCamera();
			LastCamera = MainCamera;

			base.Spawn();
		}

		public override void Respawn()
        {  
			SetModel( "models/character/pc/female/nosferatu/armor0/nosferatu_female_armor_0.vmdl" );

			Controller = new VampireWalkController();
            Animator = new VampirePlayerAnimator();
            Camera = new FirstPersonCamera();

			MainCamera = LastCamera;
			Camera = MainCamera;

			if ( DevController is NoclipController )
			{
				DevController = null;
			}

            EnableAllCollisions = true;
            EnableDrawing = true;
            EnableHideInFirstPerson = true;
            EnableShadowInFirstPerson = true;

            Health = 100;

            ClearAmmo();

            base.Respawn();
        }
        public override void OnKilled()
        {
            base.OnKilled();

			if ( LastDamage.Flags.HasFlag( DamageFlags.Vehicle ) )
			{
				Particles.Create( "particles/impact.flesh.bloodpuff-big.vpcf", LastDamage.Position );
				Particles.Create( "particles/impact.flesh-big.vpcf", LastDamage.Position );
				PlaySound( "kersplat" );
			}

			VehicleController = null;
			VehicleAnimator = null;
			VehicleCamera = null;
			Vehicle = null;

            Inventory.DropActive();
            Inventory.DeleteContents();

            BecomeRagdollOnClient(LastDamage.Force, GetHitboxBone(LastDamage.HitboxIndex));

            Controller = null;
            Camera = new SpectateRagdollCamera();

            EnableAllCollisions = false;
            EnableDrawing = false;
        }


        public override void Simulate(Client cl)
        {
            //if ( cl.NetworkIdent == 1 )
            //  return;

            base.Simulate(cl);

            // Input requested a weapon switch
            if (Input.ActiveChild != null)
            {
                ActiveChild = Input.ActiveChild;
            }

            if (LifeState != LifeState.Alive)
                return;

			if ( VehicleController != null && DevController is NoclipController )
			{
				DevController = null;
			}

			var controller = GetActiveController();
			if ( controller != null )
				EnableSolidCollisions = !controller.HasTag( "noclip" );

			TickPlayerUse();

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( MainCamera is not FirstPersonCamera )
				{
					MainCamera = new FirstPersonCamera();
				}
				else
				{
					MainCamera = new VampireThirdPersonCamera();
				}
			}

			if (Input.Pressed(InputButton.Flashlight))
			{
				if ( !Host.IsServer )
					return;

				using ( Prediction.Off() )
				{

					if ( !FoundCamEnt() )
					{
						DebugOverlay.ScreenText( new Vector2( Screen.Width / 2 + 10, Screen.Height / 2 + 20 ), 0, Color.Yellow, "Use Camera Spawner first!", 1 );
						return;
					}

					ToggleCamera();
				}
			}

			Camera = GetActiveCamera();

            if (Input.Pressed(InputButton.Drop))
            {
                var dropped = Inventory.DropActive();
                if (dropped != null)
                {
                    if (dropped.PhysicsGroup != null)
                    {
                        dropped.PhysicsGroup.Velocity = Velocity + (EyeRot.Forward + EyeRot.Up) * 300;
                    }

                    timeSinceDropped = 0;
                    SwitchToBestWeapon();
                }
            }

            SimulateActiveChild(cl, ActiveChild);

            //
            // If the current weapon is out of ammo and we last fired it over half a second ago
            // lets try to switch to a better wepaon
            //
            if (ActiveChild is WeaponBase weapon && !weapon.IsUsable() && weapon.TimeSincePrimaryAttack > 0.5f && weapon.TimeSinceSecondaryAttack > 0.5f)
            {
                SwitchToBestWeapon();
            }
        }

        public void SwitchToBestWeapon()
        {
            var best = Children.Select(x => x as WeaponBase)
                .Where(x => x.IsValid() && x.IsUsable())
                .OrderByDescending(x => x.BucketWeight)
                .FirstOrDefault();

            if (best == null) return;

            ActiveChild = best;
        }

        public override void StartTouch(Entity other)
        {
            if (timeSinceDropped < 1) return;

            base.StartTouch(other);
        }

        Rotation lastCameraRot = Rotation.Identity;

        public override void PostCameraSetup(ref CameraSetup setup)
        {
            base.PostCameraSetup(ref setup);

            if (lastCameraRot == Rotation.Identity)
                lastCameraRot = setup.Rotation;

            var angleDiff = Rotation.Difference(lastCameraRot, setup.Rotation);
            var angleDiffDegrees = angleDiff.Angle();
            var allowance = 20.0f;

            if (angleDiffDegrees > allowance)
            {
                // We could have a function that clamps a rotation to within x degrees of another rotation?
                lastCameraRot = Rotation.Lerp(lastCameraRot, setup.Rotation, 1.0f - (allowance / angleDiffDegrees));
            }
            else
            {
                //lastCameraRot = Rotation.Lerp( lastCameraRot, Camera.Rotation, Time.Delta * 0.2f * angleDiffDegrees );
            }
        }

        DamageInfo LastDamage;

        public override void TakeDamage(DamageInfo info)
        {
            LastDamage = info;

            if (GetHitboxGroup(info.HitboxIndex) == 1)
            {
                info.Damage *= 2.0f;
            }

            base.TakeDamage(info);

            if (info.Attacker is PlayerBase attacker && attacker != this)
            {
                // Note - sending this only to the attacker!
                attacker.DidDamage(To.Single(attacker), info.Position, info.Damage, Health, ((float)Health).LerpInverse(100, 0));

                // Hitmarker
                var weapon = info.Weapon as WeaponBase;
                if (weapon != null && weapon.DrawHitmarker)
                    attacker.ShowHitmarker(To.Single(attacker), Alive(), weapon.PlayHitmarkerSound);

                TookDamage(To.Single(this), info.Weapon.IsValid() ? info.Weapon.Position : info.Attacker.Position);
            }
        }

        public bool Alive()
        {
            return Health <= 0;
        }

        public bool InFirstPerson()
        {
            return Owner.Camera != null ? Owner.Camera is FirstPersonCamera : true;
        }

        [ClientRpc]
        public void DidDamage(Vector3 pos, float amount, float health, float healthinv)
        {
            Sound.FromScreen("dm.ui_attacker")
                .SetPitch(1 + healthinv * 1);

        }

        [ClientRpc]
        public void TookDamage(Vector3 pos)
        {
            //DebugOverlay.Sphere( pos, 5.0f, Color.Red, false, 50.0f );

            DamageIndicator.Current?.OnHit(pos);
        }

        [ClientRpc]
        public void ShowHitmarker(bool isKill, bool playSound)
        {
            Hitmarker.Current?.Create(isKill);

            if (playSound)
                PlaySound("swb_hitmarker");
        }

		public override PawnController GetActiveController()
		{
			if ( VehicleController != null ) return VehicleController;
			if ( DevController != null ) return DevController;

			return base.GetActiveController();
		}

		public override PawnAnimator GetActiveAnimator()
		{
			if ( VehicleAnimator != null ) return VehicleAnimator;

			return base.GetActiveAnimator();
		}

		public ICamera GetActiveCamera()
		{
			if ( VehicleCamera != null ) return VehicleCamera;

			return MainCamera;
		}

		private void ToggleCamera()
		{
			if ( !FoundCamEnt() )
			{
				return;
			}

			if ( Owner is VampirePlayer player )
			{
				if ( player.MainCamera is ToolCamera )
				{
					player.MainCamera = new FirstPersonCamera();
				}
				else
				{
					player.MainCamera = new ToolCamera();
				}
			}
		}

		private bool FoundCamEnt()
		{
			//There has to be a better way of checking if the player has spawned an entity
			//since we don't support multiple cameras we return the first one that matches the owner
			for ( int i = 0; i < Entity.All.Count; i++ )
			{
				if ( All[i] is ToolCameraEntity ) // && entity.Owner == this.Owner )
				{
					return true;
				}
			}

			return false;
		}
	}
}
