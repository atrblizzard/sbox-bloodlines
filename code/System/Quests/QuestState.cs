using System.Collections.Generic;
using System.Linq;
using Editor;
using Sandbox;

namespace bloodlines.game.Quest
{
	public partial class QuestState : EntityComponent
	{
		public static QuestState Instance { get; private set; }

		private QuestTable _questJournal;
		[Net] public IDictionary<string, int> QuestCompletionState { get; set; }
		
		public List<QuestTable> QuestTables { get; set; } = new();

		public QuestState()
		{
			QuestCompletionState = new Dictionary<string, int>();
			ReadQuestData("vdata/quests/santamonica.quest");

			if (Instance != null) return;
			Instance = this;
		}

		[ConCmd.Server("listquests")]
		public static void Cmd_ListQuests()
		{
			foreach (var quest in Instance.GetQuests())
			{
				Log.Info(quest.Title);
			}
		}

		public List<QuestJournal> GetQuests()
		{
			return _questJournal.Quests.ToList();
		}

        [ConCmd.Client("quest_populate_test", CanBeCalledFromServer = true)]
        public static void Cmd_TestQuests()
		{
            var quests = Instance.GetQuests();
            var names = new List<string>
            {
                "Arthur Knox",
                "Prince",
                "Bloodhunt",
                "Morphine",
                "Astrolite",
                "Muddy",
                "Copper",
                "Serial",
                "Lily",
                "Danielle",
                "Hotel",
                "Tourette",
                "Strauss",
                "Warehouse"
            };
            var questList = new List<QuestJournal>();
            foreach (var name in names)
            {
                questList.Add(quests.FirstOrDefault(x => x.Title == name));
            }

            foreach (var quest in questList)
            {
                //foreach (var completionState in quest1.CompletionState.DefaultIfEmpty()
                //             .Where(x => x.Type == CompletionType.Failure))
                //{
                //    Instance.SetQuest(quest1.Title, completionState.ID);
                //}

                var a = Game.Random.Int(1, quest.CountCompletionState);
                Instance.SetQuest(quest.Title, a);
            }
        }

		[ConCmd.Client("getqueststate", CanBeCalledFromServer = true)]
        public static void Cmd_GetQuestState(string name)
		{
			var questState = Instance._questJournal.Quests.FirstOrDefault(x => x.Title == name).CompletionState
				.ToList();
			foreach (var quest in Instance.QuestCompletionState)
			{
				Log.Info("Getting Quest state of " + quest.Key + ": " + quest.Value);
				var currentState = questState.FirstOrDefault(x => x.ID == quest.Value);
				Log.Info("> " + currentState.ID + " | " + currentState.Description +
				         " | " + currentState.AwardXP + " | " + currentState.AwardMoney + " | " + currentState.Type);
			}
		}
		
		[ConCmd.Client("setqueststate", CanBeCalledFromServer = true)]
		public static void Cmd_SetQuestState(string name, int state)
		{
			if (ConsoleSystem.Caller.Pawn is not VampirePlayer target) return;

			if (ConsoleSystem.Caller == null)
				return;

			target.QuestState.SetQuest(name, state);
			Instance.SetQuest(name, state);
			Log.Info($"Quest states found: {target.QuestState.QuestCompletionState.Count}");
		}

		public void SetQuest(string name, int state)
		{
			if (Instance._questJournal == null)
			{
				Log.Warning("QuestJournal is null!");
				return;
			}

			var filteredQuest = Instance._questJournal.Quests.Where(x => x.Title == name);
			if (name == filteredQuest.FirstOrDefault().Title)
			{
				QuestCompletionState[name] = state;
				Log.Info($"{name} set to {state}");
			}
		}

		public void ReadQuestData(string path)
		{
			var dialog = ResourceLibrary.Get<QuestTable>(path);
			_questJournal = dialog;
			Log.Info(_questJournal.ResourcePath);
		}

		public void SetQuestState(int questId, int state)
		{
			if (QuestCompletionState == null)
			{
				Log.Warning("QuestCompletionState is null");
				return;
			}
			
			Log.Info(QuestCompletionState.Count);

			if (QuestCompletionState.Count != _questJournal.Quests.Count)
			{
				return;
			}

			Log.Info($"_questJournal quests count: {_questJournal.Quests.Count}");
			for (int i = 0; i < _questJournal.Quests.Count; i++)
			{
				var questJournal = _questJournal.Quests[i];
				var test = new Dictionary<QuestJournal, int>
				{
					{ _questJournal.Quests[questId], state }
				};
				var quest = test.FirstOrDefault().Key;
				quest.DisplayName = questJournal.DisplayName;
				quest.Title = questJournal.Title;
				quest.CompletionState = new List<CompletionState>();
				foreach (var compState in questJournal.CompletionState)
				{
					quest.CompletionState.Add(compState);
				}
			}
		}

		private void DebugLog(int choice)
		{
			if (_questJournal.Quests == null)
			{
				Log.Warning("Dialog entry is null!");
				return;
			}

			foreach (var quest in _questJournal.Quests)
			{
				Log.Info(quest.Title);
				Log.Info(quest.DisplayName);
				if (quest.CheckCompletionState(choice))
				{
					Log.Info(quest.CompletionState[choice].ID);
					Log.Info(quest.CompletionState[choice].Description);
					Log.Info(quest.CompletionState[choice].Type);
				}
			}
		}

		public void CreateInstance()
		{
			if (Instance == null)
			{
				Log.Info("Instance not found, creating one");
				Instance = new QuestState();
				return;
			}

			Log.Info("Found instance");
		}

		public void LoadFromClient(IClient cl)
		{
			var data = cl.GetClientData("queststate");
			Deserialize(data);
		}
		
		public string Serialize()
		{
			return System.Text.Json.JsonSerializer.Serialize( QuestCompletionState );
		}
		
		public static string QuestStateJson { get; set; }
		
		[ConCmd.Client( "quest_savestate", CanBeCalledFromServer = true )]
		private static void Cmd_SaveQuestState()
		{
			Instance.SaveChanges();
		}
		
		[ConCmd.Client( "quest_loadstate", CanBeCalledFromServer = true )]
		private static void Cmd_LoaduestState()
		{
			Instance.LoadState();
		}

		public void LoadState()
		{
			var path = $"queststate.txt";
			var jsonContent = FileSystem.Data.ReadAllText($"{path}");
			Deserialize(jsonContent);
		}

		public void SaveChanges()
		{
			var str = Serialize();
			var path = $"queststate.txt";
			FileSystem.Data.WriteAllText($"{path}", str);
			Log.Info( "Settings have been saved!" );
		}

		private void Deserialize(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return;

			try
			{
				var entries = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>( json );
				
				Log.Info("Deserializing quest state");
				QuestCompletionState.Clear();
				
				foreach ( var entry in entries )
				{
					var item = entry;
					if ( item.Key == null ) continue;
					Add( item );
				}
			}
			catch (System.Exception e)
			{
				Log.Warning(e, "Error deserializing quest stable");
			}
		}

		private void Add(KeyValuePair<string, int> keyValuePair)
		{
			QuestCompletionState.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}
}