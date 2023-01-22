using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	public class HackingCamera : Entity
	{
		public Entity Owner { get; set; }
		private static bool drawHud { get; set; } = false;	
	}
}
