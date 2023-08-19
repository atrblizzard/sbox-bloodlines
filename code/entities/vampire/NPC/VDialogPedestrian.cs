using Editor;
using Sandbox;
using Vampire.ObsoleteClass;

namespace Bloodlines.Entities.Vampire.NPC;

[Library( "npc_VDialogPedestrian" )]
[HammerEntity]
[EditorModel( "models/rust_player/rustplayer.vmdl" )]
[Model]

// A pedestrian NPC that can engage in dialogue with the player.
public class VDialogPedestrian : VPedestrian
{
    private string DialogPathTest = "vdata/dialog/jeanette.dialog";

    public override void Spawn()
	{
		base.Spawn();
	}

	public override bool OnUse(Entity user)
	{
		if (user is VampirePlayer player)
		{
			player.DialogManager.ReadDialogData(DialogPathTest);
			player.DialogManager.GetDialog();
			player.DialogManager.ShowDialog();
		}

		return false;
	}
}
