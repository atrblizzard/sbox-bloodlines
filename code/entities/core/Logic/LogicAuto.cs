﻿using Editor;
using Sandbox;

namespace Bloodlines.Entities.core.Logic;

/// <summary>
/// A logic entity that allows to do a multitude of logic operations with Map I/O.<br/>
/// <br/>
/// TODO: This is a stop-gap solution and may be removed in the future in favor of "map blueprints" or node based Map I/O.
/// </summary>
[ClassName( "logic_auto" )]
[HammerEntity]
[VisGroup( VisGroup.Logic )]
[EditorSprite("materials/editor/logic_auto.vmat")]
[Title( "Logic Auto" ), Category( "Logic" ), Icon( "calculate" )]
public partial class LogicAuto : Entity
{
	/// <summary>
	/// The (initial) enabled state of the logic entity.
	/// </summary>
	[Property]
	public bool Enabled { get; set; } = true;

	/// <summary>
	/// Enables the entity.
	/// </summary>
	[Input]
	public void Enable()
	{
		Enabled = true;
	}

	/// <summary>
	/// Disables the entity, so that it would not fire any outputs.
	/// </summary>
	[Input]
	public void Disable()
	{
		Enabled = false;
	}

	/// <summary>
	/// Toggles the enabled state of the entity.
	/// </summary>
	[Input]
	public void Toggle()
	{
		Enabled = !Enabled;
	}

	/// <summary>
	/// Fired after all map entities have spawned, even if it is disabled.
	/// </summary>
	protected Output OnMapSpawn { get; set; }

	/// <summary>
	/// Fired after all map entities have spawned, even if it is disabled.
	/// </summary>
	[GameEvent.Entity.PostSpawn]
	public void OnMapSpawnEvent()
	{
		if ( !Enabled ) return;
		Log.Info("Activating logic auto");
		OnMapSpawn.Fire( this );
		Enabled = false;
	}
	[GameEvent.Entity.PostCleanup]
    public void OnMapCleanupEvent()
    {
        //Log.Info("Deactivating logic auto by " + activator);
        OnMapSpawn.Fire( this );
    }
}
