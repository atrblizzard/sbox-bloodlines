using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Vampire.Data.Quest
{
	[GameResource("Quest Table", "quest", "VTM:B Quest Definitions")]
    public partial class QuestTable : GameResource
    {
	    public List<QuestJournal> Quests { get; set; } = new();

	    [HideInEditor]
	    [JsonIgnore]
	    public int Count => Quests.Count;

		/// <summary>
        /// Add quest
        /// </summary>
        /// <param name="quest"></param>
        public void AddQuest(QuestJournal quest)
        {
            Quests.Add(quest);
        }

        /// <summary>
        /// Add state completion to quest
        /// </summary>
        /// <param name="quest"></param>
        public void AddCompletion(QuestJournal quest)
        {
            Quests.Add(quest);
        }

        public void Insert(int index, QuestJournal quest)
        {
            Quests.Insert(index, quest);
        }

        public void Remove(QuestJournal quest)
        {
            Quests.Remove(quest);
        }

        public void Remove(int index)
        {
            Quests.RemoveAt(index);
        }

        public string GetQuest(int index)
        {
            return Quests.ElementAt(index).Title;
        }

        public QuestJournal GetQuestIndex(int index)
        {
            return Quests.ElementAt(index);
        }

        public CompletionState GetStateIndex(QuestJournal quest, int index, int stateIndex)
        {
            return Quests.ElementAt(index).GetCompletionStateForQuest(quest, stateIndex);
        }

        public void RemoveAt(int index)
        {
            Quests.RemoveAt(index);
        }
    }
}
