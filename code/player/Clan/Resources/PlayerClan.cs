using Amper.FPS;
using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Vampire
{
	[GameResource("Vampire Player Clan", "vclan", "VTMB player clan definition", Icon = "accessibility_new",
		IconBgColor = "#ffc84f", IconFgColor = "#0e0e0e")]
	public class PlayerClan : GameResource
	{
		public static Dictionary<string, PlayerClan> All { get; set; } = new();

		public static Dictionary<VampirePlayerClan, string> Names { get; set; } = new()
		{
			{ VampirePlayerClan.Undefined, "undefined" },
			{ VampirePlayerClan.Brujah, "brujah" },
			{ VampirePlayerClan.Gangrel, "gangrel" },
			{ VampirePlayerClan.Malkavian, "malkavian" },
			{ VampirePlayerClan.Nosferatu, "nosferatu" },
			{ VampirePlayerClan.Toreador, "toreador" },
			{ VampirePlayerClan.Tremere, "tremere" },
			{ VampirePlayerClan.Ventrue, "ventrue" },
			{ VampirePlayerClan.CustomClan1, "customclan1" },
			{ VampirePlayerClan.CustomClan2, "customclan2" }
		};

		public static Dictionary<string, VampirePlayerClan> Entries { get; set; } =
			Names.ToDictionary(x => x.Value, x => x.Key);

		[HideInEditor]
		public VampirePlayerClan Entry =>
			Entries.TryGetValue(ResourceName, out var entry) ? entry : VampirePlayerClan.Undefined;

		public string Title { get; set; }

		public string Description { get; set; }

		public float MaxHealth { get; set; }
		public float MaxSpeed { get; set; }
		public float EyeHeight { get; set; } = 75;

		[ResourceType("vmdl")] public string Model { get; set; }
		[ResourceType("vmdl")] public string Hands { get; set; }

		public string AnimationFile { get; set; }

		public PlayerClassAbilities Abilities { get; set; }

		public struct PlayerClassAbilities
		{
			public float AutoRegenHealth { get; set; }
		}

		protected override void PostLoad()
		{
			base.PostLoad();
			Precache.Add(Model);
			Precache.Add(Hands);

			// Get lowercase class name
			string classname = ResourceName.ToLower();
			All[classname] = this;
		}

		public static bool IsValid(string name)
		{
			name = name.ToLower();

			return All.ContainsKey(name);
		}

		public static PlayerClan Get(string name)
		{
			name = name.ToLower();

			if (!IsValid(name)) return null;

			return All[name];
		}

		public static PlayerClan Get(VampirePlayerClan pclass)
		{
			string name = Names[pclass];
			if (!IsValid(name)) return null;

			return All[name];
		}

		public string GetTag()
		{
			return $"class_{ResourceName}";
		}
	}
}