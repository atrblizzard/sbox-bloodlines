﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class VampireMPGame
{
	static async Task<string> SpawnPackageModel(string packageName, Vector3 pos, Rotation rotation, Entity source)
	{
		var package = await Package.Fetch(packageName, false);
		if (package == null || package.PackageType != Package.Type.Model || package.Revision == null)
		{
			// spawn error particles
			return null;
		}

		if (!source.IsValid) return null; // source entity died or disconnected or something

		var model = package.GetMeta("PrimaryAsset", "models/dev/error.vmdl");
		var mins = package.GetMeta("RenderMins", Vector3.Zero);
		var maxs = package.GetMeta("RenderMaxs", Vector3.Zero);

		// downloads if not downloads, mounts if not mounted
		await package.MountAsync();

		return model;
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

		//
		// Does this look like a package?
		//
		if (modelname.Count(x => x == '.') == 1 && !modelname.EndsWith(".vmdl", System.StringComparison.OrdinalIgnoreCase) && !modelname.EndsWith(".vmdl_c", System.StringComparison.OrdinalIgnoreCase))
		{
			modelname = await SpawnPackageModel(modelname, tr.EndPosition, modelRotation, owner as Entity);
			if (modelname == null)
				return;
		}

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

		Sandbox.Services.Stats.Increment(owner.Client, "spawn.model", 1, modelname);
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

		//Log.Info( $"ent: {ent}" );
	}

	[ConCmd.Server("spawnpackage")]
	public static async Task SpawnPackage(string fullIdent)
	{
		var owner = ConsoleSystem.Caller.Pawn as Player;

		if (owner == null)
			return;

		Log.Info($"Spawn package {fullIdent}");

		var package = await Package.FetchAsync(fullIdent, false);
		if (package == null)
		{
			Log.Warning($"Tried to spawn package {fullIdent} - which was not found");
			return;
		}

		Log.Info($"Spawn package {package.Title}");

		var entityname = package.GetMeta("PrimaryAsset", "");

		if (string.IsNullOrEmpty(entityname))
		{
			Log.Warning($"{package.FullIdent} doesn't have a PrimaryAsset key");
			return;
		}

		if (!CanSpawnPackage(package))
		{
			Log.Warning($"Not allowed to spawn package {package.FullIdent}");
			return;
		}

		await package.MountAsync(true);

		Log.Info($"Spawning Entity: {entityname}");

		var type = TypeLibrary.GetType(entityname);
		if (type == null)
		{
			Log.Warning($"'{entityname}' type wasn't found for {package.FullIdent}");
			return;
		}

		Log.Info($"Found Type: {type.Name}");
		Log.Info($"		  : {type.ClassName}");

		var ent = type.Create<Entity>();

		var tr = Trace.Ray(owner.EyePosition, owner.EyePosition + owner.EyeRotation.Forward * 200)
							.UseHitboxes()
							.Ignore(owner)
							.Size(2)
							.Run();

		ent.Position = tr.EndPosition;
		ent.Rotation = Rotation.From(new Angles(0, owner.EyeRotation.Angles().yaw, 0));
	}

	static bool CanSpawnPackage(Package package)
	{
		if (package.PackageType != Package.Type.Addon) return false;
		if (!package.Tags.Contains("runtime")) return false;

		return true;
	}
}
