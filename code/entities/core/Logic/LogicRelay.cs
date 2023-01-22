using Editor;
using Sandbox;

namespace Bloodlines.Entities.core.Logic;

[Library( "logic_relay" )]
[HammerEntity]
[VisGroup( VisGroup.Logic )]
[EditorSprite("materials/editor/logic_relay.vmat")]
[Title( "Logic Relay" ), Category( "Logic" ), Icon( "calculate" )]
public partial class LogicRelay : Entity
{
	[Property]
	public bool Enabled { get; set; }
	
	[Property]
	public bool TriggerOnce { get; set; }
	
	[Property]
	public bool FastRetrigger { get; set; }
	
	/// <summary>
	/// Fired when the this entity receives the "Trigger" input.
	/// </summary>
	protected Output OnTrigger { get; set; }

	/// <summary>
	/// Trigger the "OnTrigger" output.
	/// </summary>
	[Input]
	public void Trigger()
	{
		if ( !Enabled ) return;

		//if (activator is not LogicRelay) //prevent dumb crash
		//{
		//Log.Info("Activating logic relay by " + activator);
		OnTrigger.Fire( this );
		//}
	}
}
