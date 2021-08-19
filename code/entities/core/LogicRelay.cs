using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.entities.core
{
	/// <summary>
	/// A generic relay that is useful to control other map entities via map input/outputs.
	/// </summary>
	[Library( "logic_relay" )]

	public partial class LogicRelay : BaseTrigger
	{
		/// <summary>
		/// Called once at least a single entity that passes filters is touching this trigger, just before this trigger getting deleted
		/// </summary>
		protected Output OnTrigger { get; set; }

		public virtual void OnTriggered( Entity other )
		{
			OnTrigger.Fire( other );
		}

		[Input]
		protected void Trigger( Entity activator )
		{
			OnTrigger.Fire( activator );
			Log.Info( activator );
		}
	}
}
