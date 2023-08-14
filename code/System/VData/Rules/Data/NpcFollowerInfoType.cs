namespace Vampire.System.VData.Rules.Data
{
    public struct NpcFollowerInfoType
    {
        /// <summary>
        /// This is the distance at which a follower begins to back away from its boss (in game units)
        /// </summary>
        public float DistanceBackaway { get; set; }

        /// <summary>
        /// This is the distance at which a follower begins to walk towards its boss (in game units)
        /// </summary>
        public float DistanceWalkTo { get; set; }

        /// <summary>
        /// This is the distance at which a follower begins to run towards its boss (in game units)
        /// </summary>
        public float DistanceRunTo { get; set; }
    }
}