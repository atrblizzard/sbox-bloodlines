﻿using Sandbox;
using Editor;

namespace Bloodlines.Entities.Vampire.NPC
{
	[Library("npc_VVampire")]
	[HammerEntity]
	[Model]
	[EditorModel("models/editor/playerstart.vmdl")]
	public partial class VVampire : VBaseNPC
	{
		private string DialogPathTest = "vdata/dialog/jeanette.dialog";
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
}