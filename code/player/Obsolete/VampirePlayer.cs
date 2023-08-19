using Sandbox;
using Bloodlines.Systems.Dialog;
using System;
using System.Collections.Generic;
using Bloodlines.Systems.Quest;
using Vampire.CameraEffects;
using Vampire.Data.Quest;
using Vampire.entities.vampire;
using Vampire.System.VData.Character.Attributes.Data;
using Trace = Sandbox.Trace;

namespace Vampire.ObsoleteClass;

public partial class VampirePlayer : Player
{
    public class VCharacterAttributes
    {
        public Dictionary<string, int> Skills { get; set; }
        public Dictionary<string, int> Feats { get; set; }
        public Dictionary<string, int> Disciplines { get; set; }
    }
    
    private TimeSince timeSinceDropped;
    private TimeSince timeSinceJumpReleased;

    private DamageInfo lastDamage;

    [Net, Predicted]
    public bool ThirdPersonCamera { get; set; }

    [ConVar.Server("thirdperson_offset_x")]
    public float TPSOffsetX = 10;
    [ConVar.Server("thirdperson_offset_y")]
    public float TPSOffsetY = 85;
    [ConVar.Server("thirdperson_offset_z")]
    public float TPSOffsetZ = 64;

    /// <summary>
    /// The clothing container is what dresses the citizen
    /// </summary>
    public ClothingContainer Clothing = new();

	// Components
	[Net] public QuestState QuestState { get; set; }
	[Net] public DialogManager DialogManager { get; set; }

    //[Net] 
    public ObserverCamera ObserverCamera { get; set; }

	public static VampirePlayer Me => Game.LocalPawn as VampirePlayer;

    public VAttributes Attributes { get; set; } = new VAttributes();
    public VCharacterAttributes CharacterAttributes { get; set; } = new();
    
    [Net] public int UniqueRandomSeed { get; set; }
    
    private float WalkBob;
    private float Lean;
    private float FOV;

    public string Clan { get; set; }

    /// <summary>
    /// Default init
    /// </summary>
    public VampirePlayer()
    {
        Inventory = new Inventory(this);
	}

    private void SetupComponents()
    {
		var dialogManager = Components.GetOrCreate<DialogManager>();
		var questState = Components.GetOrCreate<QuestState>();

        DialogManager = dialogManager;
        QuestState = questState;
        ObserverCamera = new ObserverCamera();
        ObserverCamera = Components.GetOrCreate<ObserverCamera>();
        
    }

	private void AddCameraEffects(bool noclip)
    {
        var speed = Velocity.Length.LerpInverse( 0f, 320f );
        var forwardSpeed = Velocity.Normal.Dot( Camera.Rotation.Forward );

        var left = Camera.Rotation.Left;
        var up = Camera.Rotation.Up;

        if ( GroundEntity != null )
        {
            WalkBob += Time.Delta * 25f * speed;
        }

        Camera.Position += up * MathF.Sin( WalkBob ) * speed * 2f;
        Camera.Position += left * MathF.Sin( WalkBob * 0.6f ) * speed * 1f;

        Lean = Lean.LerpTo( Velocity.Dot( Camera.Rotation.Right ) * 0.01f, Time.Delta * 15f );

        var appliedLean = Lean;
        appliedLean += MathF.Sin( WalkBob ) * speed * 0.3f;
        Camera.Rotation *= Rotation.From( 0, 0, appliedLean );

        speed = (speed - 0.7f).Clamp( 0f, 1f ) * 3f;

        if (!noclip)
            FOV = FOV.LerpTo( speed * 20f * MathF.Abs( forwardSpeed ), Time.Delta * 4f );

        Camera.FieldOfView += FOV;
    }

    /// <summary>
    /// Initialize using this client
    /// </summary>
    public VampirePlayer(IClient cl) : this()
    {
        // Load clothing from client data
        Clothing.LoadFromClient(cl);
        QuestState.LoadFromClient(cl);
    }

    [ConCmd.Server("questtest")]
    public static void QuestTest()
    {
        var target = ConsoleSystem.Caller.Pawn as VampirePlayer;
        if (target == null) return;
        
        Log.Info(target.QuestState.QuestCompletionState.Count);
        
        foreach (var quest in target.QuestState.QuestCompletionState)
            Log.Info(quest.Key + ": " + quest.Value);
    }
    
    [ConCmd.Server("setqueststateplayer")]
    public static void Cmd_SetQuestState(string name, int state)
    {
        if (ConsoleSystem.Caller.Pawn is not VampirePlayer target) return;

        if (ConsoleSystem.Caller == null)
            return;
      
        
        target.QuestState.SetQuest(name, state);
        
        
        Log.Info(target.QuestState.QuestCompletionState.Count);
    }

    [ClientRpc]
    public static void Cl_SetClan(string name)
    {
        if (string.IsNullOrEmpty(name)) return;

        Me.Clan = name;
        Log.Info("Set player clan to: " + name);
    }

#if TEST
    [ConCmd.Client("setclan")]
    public static void Cmd_SetClan(string name)
    {
        if (ConsoleSystem.Caller.Pawn is not VampirePlayer target) return;

        if (ConsoleSystem.Caller == null)
            return;

        if (string.IsNullOrEmpty(name)) return;

        target.Clan = name;
        Log.Info($"Set player clan to: {name}");

        Cl_SetClan(name);
    }
#endif

    public object GetAttribute(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var props = TypeLibrary.GetPropertyDescriptions(Attributes, true);
        foreach (var prop in props)
        {
            var def = prop.GetValue(Attributes);
            if (def != null)
            {
                foreach (var defProp in TypeLibrary.GetPropertyDescriptions(def, true))
                {
                    if (defProp.Name == name)
                    {
                        var value = defProp.GetValue(def);
                        //Log.Info(value.ToString());
                        return value;
                    }
                }
            }
        }

        return null;
    }

#if TEST    
    [ConCmd.Client("getattribute", CanBeCalledFromServer = true)]
    public static void Cmd_GetAttributes(string name)
    {
        if (ConsoleSystem.Caller == null)
            return;

        if (string.IsNullOrEmpty(name)) return;

        Log.Info(Me.GetAttribute(name));
    }


    [ConCmd.Client("setattribute", CanBeCalledFromServer = true)]
    public static void Cmd_SetAttributes(string name, int value)
    {
        if (ConsoleSystem.Caller == null)
            return;

        Me.SetAttribute(name, value);
    }
#endif

    public void SetAttribute(string name, int value)
    {
        if (string.IsNullOrEmpty(name)) return;

        var props = TypeLibrary.GetPropertyDescriptions(Attributes, true);
        foreach (var prop in props)
        {
            var def = prop.GetValue(Attributes);
            if (def != null)
            {
                foreach (var defProp in TypeLibrary.GetPropertyDescriptions(def, true))
                {
                    if (defProp.Name == name)
                    {
                        defProp.SetValue(def, value);
                        Log.Info($"Set attribute {defProp.Name} to value: {defProp.GetValue(def)}");
                    }
                }
            }
        }
    }

    public override void Spawn()
    {
		SetupComponents();

		base.Spawn();

        EnableDrawing = true;
        EnableHideInFirstPerson = true;
        EnableShadowInFirstPerson = true;

        QuestState?.LoadState();
    }

    public override void Respawn()
    {
        var path = @"models\Character\pc\female\tremere\armor0\tremere_female_armor_0.vmdl";
        
        // Override path for regular citizen model
        path =@"models/citizen/citizen.vmdl";
        if (FileSystem.Mounted.FileExists(path))
        {
            SetModel(path);
        }
        else
        {
            Log.Warning($"File does not exist! (Path: {path})");
        }
        
        RemoveRagdollEntity();

        Controller = new WalkController();

        if (DevController is NoclipController)
        {
            DevController = null;
        }

        this.ClearWaterLevel();
        EnableAllCollisions = true;
        EnableDrawing = true;
        EnableHideInFirstPerson = true;
        EnableShadowInFirstPerson = true;
        
        Health = 100f;
        LifeState = LifeState.Alive;

        Clothing.DressEntity(this);

        base.Respawn();
    }

    private void RemoveRagdollEntity()
    {
        // TODO: implement removal of ragdoll entity after spawning
    }

    public override void OnKilled()
    {
        base.OnKilled();

        if (lastDamage.HasTag("vehicle"))
        {
            Particles.Create("particles/impact.flesh.bloodpuff-big.vpcf", lastDamage.Position);
            Particles.Create("particles/impact.flesh-big.vpcf", lastDamage.Position);
            PlaySound("kersplat");
        }

        BecomeRagdollOnClient(Velocity, lastDamage.Position, lastDamage.Force, lastDamage.BoneIndex, lastDamage.HasTag("bullet"), lastDamage.HasTag("blast"));

        Controller = null;

        EnableAllCollisions = false;
        EnableDrawing = false;

        foreach (var child in Children)
        {
            child.EnableDrawing = false;
        }

        Inventory.DropActive();
        Inventory.DeleteContents();
    }

    public override void TakeDamage(DamageInfo info)
    {
        if (info.Attacker.IsValid())
        {
            //if (info.Attacker.Tags.Has($"{PhysGun.GrabbedTag}{Client.SteamId}"))
            //    return;
        }

        if (info.Hitbox.HasTag("head"))
        {
            info.Damage *= 10.0f;
        }

        lastDamage = info;

        base.TakeDamage(info);
    }

    public override PawnController GetActiveController()
    {
        if (DevController != null)
            return DevController;

        return base.GetActiveController();
    }

    public override void Simulate(IClient cl)
    {
        base.Simulate(cl);

        if (LifeState != LifeState.Alive)
            return;

        var controller = GetActiveController();
        if (controller != null)
        {
            EnableSolidCollisions = !controller.HasTag("noclip");

            SimulateAnimation(controller);
        }

        TickPlayerUse();
        SimulateActiveChild(cl, ActiveChild);
        
        if (Input.Pressed(InputButton.View))
        {
            ThirdPersonCamera = !ThirdPersonCamera;
        }

        if (Input.Pressed(InputButton.Drop))
        {
            var dropped = Inventory.DropActive();
            if (dropped != null)
            {
                dropped.PhysicsGroup.ApplyImpulse(Velocity + EyeRotation.Forward * 500.0f + Vector3.Up * 100.0f, true);
                dropped.PhysicsGroup.ApplyAngularImpulse(Vector3.Random * 100.0f, true);

                timeSinceDropped = 0;
            }
        }

        if (Input.Released(InputButton.Jump))
        {
            if (timeSinceJumpReleased < 0.3f)
            {
                if (DevController is NoclipController)
                {
                    DevController = null;
                }
                else
                {
                    DevController = new NoclipController();
                }
            }

            timeSinceJumpReleased = 0;
        }

        if (InputDirection.y != 0 || InputDirection.x != 0f)
        {
            timeSinceJumpReleased = 1;
        }
    }

    [ConCmd.Admin("nonoclip")]
    static void DoPlayerNoclip()
    {
        if (ConsoleSystem.Caller.Pawn is VampirePlayer basePlayer)
        {
            if (basePlayer.DevController is NoclipController)
            {
                basePlayer.DevController = null;
            }
            else
            {
                basePlayer.DevController = new NoclipController();
            }
        }
    }

    [ConCmd.Admin("kill")]
    static void DoPlayerSuicide()
    {
        if (ConsoleSystem.Caller.Pawn is VampirePlayer basePlayer)
        {
            basePlayer.TakeDamage(new DamageInfo { Damage = basePlayer.Health * 99 });
        }
    }


    Entity lastWeapon;

    void SimulateAnimation(PawnController controller)
    {
        if (controller == null)
            return;

        // where should we be rotated to
        var turnSpeed = 0.02f;

        var rotation =
            // If we're a bot, spin us around 180 degrees.
            Client.IsBot ? ViewAngles.WithYaw(ViewAngles.yaw + 180f).ToRotation() : ViewAngles.ToRotation();

        var idealRotation = Rotation.LookAt(rotation.Forward.WithZ(0), Vector3.Up);
        Rotation = Rotation.Slerp(Rotation, idealRotation, controller.WishVelocity.Length * Time.Delta * turnSpeed);
        Rotation = Rotation.Clamp(idealRotation, 45.0f, out var shuffle); // lock facing to within 45 degrees of look direction

        CitizenAnimationHelper animHelper = new CitizenAnimationHelper(this);

        animHelper.WithWishVelocity(controller.WishVelocity);
        animHelper.WithVelocity(controller.Velocity);
        animHelper.WithLookAt(EyePosition + EyeRotation.Forward * 100.0f, 1.0f, 1.0f, 0.5f);
        animHelper.AimAngle = rotation;
        animHelper.FootShuffle = shuffle;
        animHelper.DuckLevel = MathX.Lerp(animHelper.DuckLevel, controller.HasTag("ducked") ? 1 : 0, Time.Delta * 10.0f);
        animHelper.VoiceLevel = (Game.IsClient && Client.IsValid()) ? Client.Voice.LastHeard < 0.5f ? Client.Voice.CurrentLevel : 0.0f : 0.0f;
        animHelper.IsGrounded = GroundEntity != null;
        animHelper.IsSitting = controller.HasTag("sitting");
        animHelper.IsNoclipping = controller.HasTag("noclip");
        animHelper.IsClimbing = controller.HasTag("climbing");
        animHelper.IsSwimming = this.GetWaterLevel() >= 0.5f;
        animHelper.IsWeaponLowered = false;

        if (controller.HasEvent("jump")) animHelper.TriggerJump();
        if (ActiveChild != lastWeapon) animHelper.TriggerDeploy();

        if (ActiveChild is BaseCarriable carry)
        {
            carry.SimulateAnimator(animHelper);
        }
        else
        {
            animHelper.HoldType = CitizenAnimationHelper.HoldTypes.None;
            animHelper.AimBodyWeight = 0.5f;
        }

        lastWeapon = ActiveChild;
    }

    public override void StartTouch(Entity other)
    {
        if (timeSinceDropped < 1) return;

        base.StartTouch(other);
    }

    public override float FootstepVolume()
    {
        return Velocity.WithZ(0).Length.LerpInverse(0.0f, 200.0f) * 5.0f;
    }

    [ConCmd.Server("inventory_current")]
    public static void SetInventoryCurrent(string entName)
    {
        var target = ConsoleSystem.Caller.Pawn as Player;
        if (target == null) return;

        var inventory = target.Inventory;
        if (inventory == null)
            return;

        for (int i = 0; i < inventory.Count(); ++i)
        {
            var slot = inventory.GetSlot(i);
            if (!slot.IsValid())
                continue;

            if (slot.ClassName != entName)
                continue;

            inventory.SetActiveSlot(i, false);

            break;
        }
    }

    public override void FrameSimulate(IClient cl)
    {
        base.FrameSimulate(cl);
        Camera.Rotation = ViewAngles.ToRotation();
        Camera.FieldOfView = Screen.CreateVerticalFieldOfView(Game.Preferences.FieldOfView);

        switch (ThirdPersonCamera)
        {
            case true:
            {
                Camera.FirstPersonViewer = null;

                Vector3 targetPos;
                var center = Position + Vector3.Up * 64;

                var pos = center;
                var rot = Camera.Rotation * Rotation.FromAxis(Vector3.Up, -16);

                float distance = 130.0f * Scale;
                targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 32) * Scale);
                targetPos += rot.Forward * -distance;

                var tr = Trace.Ray(pos, targetPos)
                    .WithAnyTags("solid")
                    .Ignore(this)
                    .Radius(8)
                    .Run();

                Camera.Position = tr.EndPosition;
                break;
            }
            default:
            {
                if (LifeState != LifeState.Alive && Corpse.IsValid())
                {
                    Corpse.EnableDrawing = true;

                    var pos = Corpse.GetBoneTransform(0).Position + Vector3.Up * 10;
                    var targetPos = pos + Camera.Rotation.Backward * 100;

                    var tr = Trace.Ray(pos, targetPos)
                        .WithAnyTags("solid")
                        .Ignore(this)
                        .Radius(8)
                        .Run();

                    Camera.Position = tr.EndPosition;
                    Camera.FirstPersonViewer = null;
                }
                else
                {
                    Camera.Position = EyePosition;
                    Camera.FirstPersonViewer = this;
                    Camera.Rotation = ViewAngles.ToRotation();
                    Camera.Main.SetViewModelCamera(90f);
                    //Camera.FieldOfView = Game.Preferences.FieldOfView; // TODO: to be enabled later
                    Camera.ZNear = 1f;
                    Camera.ZFar = 5000f;

                    //AddCameraEffects(DevController is not NoclipController);

                    //ScreenShake.Apply();
                }

                break;
            }
        }
    }

    [ClientRpc]
    private void BecomeRagdollOnClient(Vector3 velocity, Vector3 forcePos, Vector3 force, int bone, bool impulse, bool blast)
    {
        var ent = new ModelEntity();
        ent.Tags.Add("ragdoll", "solid", "debris");
        ent.Position = Position;
        ent.Rotation = Rotation;
        ent.Scale = Scale;
        ent.UsePhysicsCollision = true;
        ent.EnableAllCollisions = true;
        ent.SetModel(GetModelName());
        ent.CopyBonesFrom(this);
        ent.CopyBodyGroups(this);
        ent.CopyMaterialGroup(this);
        ent.CopyMaterialOverrides(this);
        ent.TakeDecalsFrom(this);
        ent.EnableAllCollisions = true;
        ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
        ent.RenderColor = RenderColor;
        ent.PhysicsGroup.Velocity = velocity;
        ent.PhysicsEnabled = true;

        foreach (var child in Children)
        {
            if (!child.Tags.Has("clothes")) continue;
            if (child is not ModelEntity e) continue;

            var model = e.GetModelName();

            var clothing = new ModelEntity();
            clothing.SetModel(model);
            clothing.SetParent(ent, true);
            clothing.RenderColor = e.RenderColor;
            clothing.CopyBodyGroups(e);
            clothing.CopyMaterialGroup(e);
        }

        if (impulse)
        {
            PhysicsBody body = bone > 0 ? ent.GetBonePhysicsBody(bone) : null;

            if (body != null)
            {
                body.ApplyImpulseAt(forcePos, force * body.Mass);
            }
            else
            {
                ent.PhysicsGroup.ApplyImpulse(force);
            }
        }

        if (blast)
        {
            if (ent.PhysicsGroup != null)
            {
                ent.PhysicsGroup.AddVelocity((Position - (forcePos + Vector3.Down * 100.0f)).Normal * (force.Length * 0.2f));
                var angularDir = (Rotation.FromYaw(90) * force.WithZ(0).Normal).Normal;
                ent.PhysicsGroup.AddAngularVelocity(angularDir * (force.Length * 0.02f));
            }
        }

        Corpse = ent;

        if (IsLocalPawn)
            Corpse.EnableDrawing = false;

        ent.DeleteAsync(10.0f);
    }
}