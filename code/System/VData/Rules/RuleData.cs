using Sandbox;
using System.Collections.Generic;
using Vampire.System.VData.Rules.Data;

namespace Vampire.System.VData.Rules;

[GameResource("Rules Data", "rule", "VTM:B Rules Definitions")]
public class RuleData : GameResource
{
    public List<Table> Tables { get; set; }
    public VampFrenzyInfo VampFrenzyInfo { get; set; }
    public Data.DamageInfo DamageInfo { get; set; }
    public MeleeReactions MeleeReactions { get; set; }
    public NpcFollowerInfo NpcFollowerInfo { get; set; }
}