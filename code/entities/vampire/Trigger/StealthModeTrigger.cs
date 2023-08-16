﻿using Sandbox;
using Editor;

namespace Bloodlines.Entities.Vampire.Trigger
{
	[Solid]
	[HammerEntity]
	[Library( "trigger_stealth_mod" )]
	public partial class StealthModeTrigger : BaseTrigger, IUse
	{
		[Property( "stealth_modifier", Title = "Stealth Modifier" )]
		public int StealthModifier { get; set; }

		private bool isUsing;

		/// <summary>
		/// Determines what to do when using the entity began.
		/// </summary>
		protected Output OnUseBegin { get; set; }

		/// <summary>
		/// Determines what to do when using the entity over."
		/// </summary>
		protected Output OnUseEnd { get; set; }

		[GameEvent.Tick.Server]
		protected virtual void DealDamagePerTick()
		{
			if ( !Enabled )
				return;

			foreach ( var entity in TouchingEntities )
			{
				if ( !entity.IsValid() )
					continue;

				if ( entity.Tags.Has( "player" ) )
				{
					OnUse( entity );

					OnUseBegin.Fire( entity );
    }
			}
		}

		public bool OnUse( Entity user )
		{
			if ( isUsing )
			{
				isUsing = false;
				OnUseEnd.Fire( user );
			}
			else
			{
				isUsing = true;
				OnUseBegin.Fire( user );
			}

			//TODO: Implement stealth mode mechanism in player codebase and NPC detection
			return false;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
