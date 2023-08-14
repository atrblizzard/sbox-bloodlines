namespace bloodlines.game.Quest
{
    public struct CompletionState
    {
        /// <summary>
        /// Each completion state must have a UNIQUE, NUMERIC ID. By default, a player is considered to be at 
        /// completion state 0, when the quest is unassigned. The quest will not be displayed in the journal
        /// unless the player has a valid ID for the quest.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// This is what will be displayed as the journal entry for this quest.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This controls the font, colors, etc, for the quest. MUST be "success, failure or incomplete."
        /// </summary>
        public CompletionType Type { get; set; }

        /// <summary>
        /// This is how many experience points to award to a character when they reach this quest completion level.
        /// Defaults to 0.
        /// </summary>
        public int AwardXP { get; set; }

        /// <summary>
        /// This is how much money to award to a character when they reach this quest completion level. 
        /// Defaults to 0.
        /// </summary>
        public int AwardMoney { get; set; }

        /// <summary>
        /// Completion State constructor
        /// </summary>
        public CompletionState()
        {
            ID = 0;
            Description = "!!DESCRIPTION!!";
            Type = CompletionType.Unassigned;
            AwardMoney = 0;
            AwardXP = 0;
        }

        /// <summary>
        /// Completion State fully featured constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="awardXP"></param>
        /// <param name="awardMoney"></param>
        public CompletionState(int id, string description, CompletionType type, int awardXP, int awardMoney)
        {
            ID = id;
            Description = description;
            Type = type;
            AwardXP = awardXP;
            AwardMoney = awardMoney;
        }
        
        public bool IsValid => ID != 0;
    }
}
