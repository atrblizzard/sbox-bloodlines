﻿using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor;

namespace bloodlines.entities.core.Func
{
	[HammerEntity]
	[Solid]
	[Library( "func_breakable_surf" )]
	public partial class BreakableSurface : ModelEntity
	{
		[Property( Title = "Start Disabled" )]
		public bool StartDisabled { get; set; } = false;

		public override void Spawn()
		{
			base.Spawn();

			SetupPhysicsFromModel( PhysicsMotionType.Static);
		}
	}
}
