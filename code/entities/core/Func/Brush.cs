using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.entities.core.Func
{
#if TODO
	/// <summary>
	/// A generic brush/mesh that can toggle its visibilty and collisions.
	/// </summary>
	[Library( "func_brush_vtmb" )]
	[Hammer.Solid]
	[Hammer.RenderFields]
	public partial class Brush : FuncBrush
	{
		bool _startHidden;

		[Property( "StartHidden" )]
		public bool StartHidden
		{
			get { return _startHidden; }
			set
			{
				_startHidden = value;
				CheckHidden();
			}
		}

		private void CheckHidden()
		{
			if ( StartHidden )
			{
				EnableAllCollisions = true;
			}
			else
			{
				EnableAllCollisions = false;
			}

			EnableDrawing = StartHidden;
		}
	}
#endif
}
