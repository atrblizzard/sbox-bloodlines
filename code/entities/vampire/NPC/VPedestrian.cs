using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.entities.vampire.NPC
{
	[Library("npc_VPedestrian")]
	[Hammer.Model]
	public partial class VPedestrian : AnimEntity
	{
		[Property( Title = "Model Name" )]
		public string Model { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			if ( !string.IsNullOrEmpty( Model ))
			{
				Log.Info( Model );
				if ( FileSystem.Data.FileExists( Model )) //FileSystem.Data.FileExists( Model )
				{
					SetModel( Model );
					SetupPhysicsFromModel( PhysicsMotionType.Keyframed, true );
				}
				else
				{
					SetModel( "models/editor/playerstart.vmdl" );
					SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One, Vector3.One );
				}
			}
			else
			{
				SetModel( "models/dev/playerstart_tint.vmdl" );
				SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One, Vector3.One );
			}
		}
	}
}
