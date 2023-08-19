using System;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Vampire.Data.Quest;
using Vampire.ObsoleteClass;

namespace Bloodlines.UI;

public partial class QuestPanel : Panel
{
	private bool IsOpen { get; set; }
	public bool DebugMode { get; set; }
	
	private static QuestPanel Instance { get; set; }

	public static QuestPanel CreateInstance()
	{
		if (Instance == null)
		{
			Log.Info("Instance not found, creating one");
			return new QuestPanel();
		}

		GetInstance();

		Log.Info("Found instance");
		return Instance;
	}

	public static QuestPanel GetInstance() { return Instance; }

	private VampirePlayer player;
	
	public QuestPanel()
	{
		player = Sandbox.Game.LocalPawn as VampirePlayer;
		Instance = this;
	}

	public QuestDisplayModel GetQuestDisplayModel(CompletionType type)
	{
		var currentState = GetSpecificQuestType(type);
		if (player.QuestState == null)
		{
			return null;
		}
		foreach (var completionState in player.QuestState.QuestCompletionState)
		{
			var currentQuest = player.QuestState.GetQuests().FirstOrDefault(x => x.Title == completionState.Key);
			if (currentState.ID == completionState.Value &&
			    currentState.Type == type)
			{
				return new QuestDisplayModel
				{
					DisplayName = currentQuest.DisplayName,
					Description = currentState.Description
				};
			}
		}

		return null;
	}

	public CompletionState GetSpecificQuestType(CompletionType type)
	{
		if (player.QuestState != null)
		{
			foreach (var quest in player.QuestState.QuestCompletionState)
			{
				if (DebugMode)
					Log.Info($"{quest.Key} {quest.Value}");

				if (player.QuestState.GetQuests().Count > 0)
				{
					var currentQuest = player.QuestState.GetQuests().FirstOrDefault(x => x.Title == quest.Key);
					if (currentQuest.CountCompletionState > 0)
					{
						if (DebugMode)
							Log.Info($"{currentQuest.Title} {currentQuest.DisplayName}");
						var currentState = currentQuest.CompletionState.FirstOrDefault(x => x.ID == quest.Value && x.Type == type);
						if (!currentState.Equals(default(CompletionState)) && currentState.ID == quest.Value)
						{
							if (DebugMode)
								Log.Info("Found matching state ID: " + currentState.ID);
							return currentState;
						}

						if (DebugMode)
							Log.Info("No matching state ID found");
					}
				}
			}
		}

		return new CompletionState();
	}

	[ConCmd.Client("questpanel_open")]
	public static void Command_OpenDialogPanel()
	{
		if (ConsoleSystem.Caller.Pawn is VampirePlayer player)
		{
			Instance.player = player;
			if (player.QuestState != null)
			{
				foreach (var quest in player.QuestState.QuestCompletionState)
				{
					Log.Info("Getting Quest state of " + quest.Key + ": " + quest.Value);
					if (player.QuestState.GetQuests() == null)
						return;
					
					var questState = player.QuestState.GetQuests().FirstOrDefault(x => 
						x.Title == quest.Key).CompletionState.ToList();

					if (questState.Count <= 0) continue;
					var currentState = questState.FirstOrDefault(x => x.ID == quest.Value);
					Log.Info("> " + currentState.ID + " | " + currentState.Description +
					         " | " + currentState.AwardXP + " | " + currentState.AwardMoney + " | " +
					         currentState.Type);
				}
			}
			else
			{
				Log.Error("QuestState is null on player! Shouldn't happen!");
			}
		}
	}
	
	public void Toggle()
	{
		IsOpen = !IsOpen;
	}
	
	public void Open()
	{
		IsOpen = true;
	}
	
	protected override int BuildHash()
	{
		return HashCode.Combine(IsOpen, player.QuestState?.QuestCompletionState?.Count, player.QuestState?.QuestCompletionState?.Values.Count); // base.BuildHash(),
	}
}