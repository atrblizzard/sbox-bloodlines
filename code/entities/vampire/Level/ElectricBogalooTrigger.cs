using Editor;
using Sandbox;

namespace bloodlines.entities.vampire.Level
{
	[Solid]
	[Library( "trigger_electric_bugaloo" )]
	public partial class ElectricBogalooTrigger : BaseTrigger, IUse
	{
		private bool isUsing;

		/// <summary>
		/// Determines what to do when using the entity began.
		/// </summary>
		protected Output OnUseBegin { get; set; }

		/// <summary>
		/// Determines what to do when using the entity over."
		/// </summary>
		protected Output OnUseEnd { get; set; }

		[Event.Tick.Server]
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

			Log.Info( "Bogaloo is using " + isUsing );

			return false;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
