using System.Collections.Generic;
using Sandbox;

namespace Vampire.System.Character.Data.Feats;

[GameResource("Feats Data", "feats", "VTM:B Feats Definitions")]
public class FeatData : GameResource
{
	//public Feats Feats { get; set; }
	public List<Feat> Feat { get; set; }
}