using Editor;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vampire.entities.core.Logic
{
	[Library("logic_case")]
	[HammerEntity]
	[VisGroup(VisGroup.Logic)]
	[EditorSprite("editor/logic_case.vmat")]
	[Title("logic_case"), Category("Logic"), Icon("calculate")]
	public partial class LogicCase : Entity
	{
		[Property] public bool Enabled { get; set; } = true;

		[Input]
		public void Enable()
		{
			Enabled = true;
		}

		[Input]
		public void Disable()
		{
			Enabled = false;
		}

		[Input]
		public void Toggle()
		{
			Enabled = !Enabled;
		}

		/// <summary>
		/// Fired when the this entity receives the "Trigger" input.
		/// </summary>
		protected Output OnTrigger { get; set; }

		/// <summary>
		/// Choose a random case
		/// </summary>
		[Input]
		public void PickRandom()
		{
			if (!Enabled) return;
			Tuple<Output, string>[] Cases =
			{
				new(OnCase01, Case01), new(OnCase02, Case02),
				new(OnCase03, Case03), new(OnCase04, Case04),
				new(OnCase05, Case05), new(OnCase06, Case06),
				new(OnCase07, Case07), new(OnCase08, Case08),
				new(OnCase09, Case09), new(OnCase10, Case10),
				new(OnCase11, Case11), new(OnCase12, Case12),
				new(OnCase13, Case13), new(OnCase14, Case14),
				new(OnCase15, Case15), new(OnCase16, Case16),
			};

			var CasesList = new List<Tuple<Output, string>>(Cases);
			var a = Game.Random.Int(1, 16);
			var b = CasesList.OrderBy(x => Game.Random.Float(0, 1000));
			foreach (var Outcase in b)
			{
				if (Outcase.Item2 != "")
				{
					Outcase.Item1.Fire(this);
					return;
				}
			}
		}

		List<Tuple<Output, string>> Shuffled = new();

		[Input]
		public void PickRandomShuffle()
		{
			if (!Enabled) return;
			Tuple<Output, string>[] Cases =
			{
				new(OnCase01, Case01), new(OnCase02, Case02),
				new(OnCase03, Case03), new(OnCase04, Case04),
				new(OnCase05, Case05), new(OnCase06, Case06),
				new(OnCase07, Case07), new(OnCase08, Case08),
				new(OnCase09, Case09), new(OnCase10, Case10),
				new(OnCase11, Case11), new(OnCase12, Case12),
				new(OnCase13, Case13), new(OnCase14, Case14),
				new(OnCase15, Case15), new(OnCase16, Case16),
			};
			if (Shuffled.Count == 0)
			{
				Shuffled = new List<Tuple<Output, string>>(Cases);
				var a = Game.Random.Int(1, 16);
				Shuffled = Shuffled.OrderBy(x => Game.Random.Float(0, 1000)).ToList();
			}

			foreach (var outcase in Shuffled)
			{
				if (outcase.Item2 != "")
				{
					outcase.Item1.Fire(this);
					Shuffled.Remove(outcase);
					return;
				}
			}
		}

		/// <summary>
		/// Run a value through the switch
		/// </summary>
		[Input]
		public void InValue(string VALUE)
		{
			if (!Enabled)
				return;

			switch (VALUE)
			{
				case "Case01":
					OnCase01.Fire(this);
					break;
				case "Case02":
					OnCase02.Fire(this);
					break;
				case "Case03":
					OnCase03.Fire(this);
					break;
				case "Case04":
					OnCase04.Fire(this);
					break;
				case "Case05":
					OnCase05.Fire(this);
					break;
				case "Case06":
					OnCase06.Fire(this);
					break;
				case "Case07":
					OnCase07.Fire(this);
					break;
				case "Case08":
					OnCase08.Fire(this);
					break;
				case "Case09":
					OnCase09.Fire(this);
					break;
				case "Case10":
					OnCase10.Fire(this);
					break;
				case "Case11":
					OnCase11.Fire(this);
					break;
				case "Case12":
					OnCase12.Fire(this);
					break;
				case "Case13":
					OnCase13.Fire(this);
					break;
				case "Case14":
					OnCase14.Fire(this);
					break;
				case "Case15":
					OnCase15.Fire(this);
					break;
				case "Case16":
					OnCase16.Fire(this);
					break;
				default:
					OnDefault.Fire(this);
					break;
			}

			// else if chain, switch cases only work with constants
			// if (VALUE == Case01)
			// {
			// 	OnCase01.Fire(this);
			// }
			// else if (VALUE == Case02)
			// {
			// 	OnCase02.Fire(this);
			// }
			// else if (VALUE == Case03)
			// {
			// 	OnCase03.Fire(this);
			// }
			// else if (VALUE == Case04)
			// {
			// 	OnCase04.Fire(this);
			// }
			// else if (VALUE == Case05)
			// {
			// 	OnCase05.Fire(this);
			// }
			// else if (VALUE == Case06)
			// {
			// 	OnCase06.Fire(this);
			// }
			// else if (VALUE == Case07)
			// {
			// 	OnCase07.Fire(this);
			// }
			// else if (VALUE == Case08)
			// {
			// 	OnCase08.Fire(this);
			// }
			// else if (VALUE == Case09)
			// {
			// 	OnCase09.Fire(this);
			// }
			// else if (VALUE == Case10)
			// {
			// 	OnCase10.Fire(this);
			// }
			// else if (VALUE == Case11)
			// {
			// 	OnCase11.Fire(this);
			// }
			// else if (VALUE == Case12)
			// {
			// 	OnCase12.Fire(this);
			// }
			// else if (VALUE == Case13)
			// {
			// 	OnCase13.Fire(this);
			// }
			// else if (VALUE == Case14)
			// {
			// 	OnCase14.Fire(this);
			// }
			// else if (VALUE == Case15)
			// {
			// 	OnCase15.Fire(this);
			// }
			// else if (VALUE == Case16)
			// {
			// 	OnCase16.Fire(this);
			// }
			// else
			// {
			// 	OnDefault.Fire(this);
			// }
		}

		[Property] public string Case01 { get; set; } = string.Empty;
		[Property] public string Case02 { get; set; } = string.Empty;
		[Property] public string Case03 { get; set; } = string.Empty;
		[Property] public string Case04 { get; set; } = string.Empty;
		[Property] public string Case05 { get; set; } = string.Empty;
		[Property] public string Case06 { get; set; } = string.Empty;
		[Property] public string Case07 { get; set; } = string.Empty;
		[Property] public string Case08 { get; set; } = string.Empty;
		[Property] public string Case09 { get; set; } = string.Empty;
		[Property] public string Case10 { get; set; } = string.Empty;
		[Property] public string Case11 { get; set; } = string.Empty;
		[Property] public string Case12 { get; set; } = string.Empty;
		[Property] public string Case13 { get; set; } = string.Empty;
		[Property] public string Case14 { get; set; } = string.Empty;
		[Property] public string Case15 { get; set; } = string.Empty;
		[Property] public string Case16 { get; set; } = string.Empty;

		private Output OnCase01 { get; set; }
		private Output OnCase02 { get; set; }
		private Output OnCase03 { get; set; }
		private Output OnCase04 { get; set; }
		private Output OnCase05 { get; set; }
		private Output OnCase06 { get; set; }
		private Output OnCase07 { get; set; }
		private Output OnCase08 { get; set; }
		private Output OnCase09 { get; set; }
		private Output OnCase10 { get; set; }
		private Output OnCase11 { get; set; }
		private Output OnCase12 { get; set; }
		private Output OnCase13 { get; set; }
		private Output OnCase14 { get; set; }
		private Output OnCase15 { get; set; }
		private Output OnCase16 { get; set; }
		private Output OnDefault { get; set; }
	}
}