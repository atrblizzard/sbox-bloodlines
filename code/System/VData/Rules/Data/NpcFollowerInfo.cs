namespace Vampire.System.VData.Rules.Data
{
    public struct NpcFollowerInfo
    {
        public NpcFollowerInfoType Default { get; set; }
        public NpcFollowerInfoType Combat { get; set; }
        public NpcFollowerInfoType CombatNonCombatant { get; set; }
    }
}