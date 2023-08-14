using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace bloodlines.game.Quest;

public struct QuestJournal
{
	/// <summary>
	/// This is a simple text title, and what will be used to refer to it in dialog.
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// This is what will be displayed as the heading for this journal entry, and can be localized.
	/// </summary>
	public string DisplayName { get; set; }
		
	public List<CompletionState> CompletionState { get; set; } = new();

	/// <summary>
	/// Count the amount of completion states within the quest.
	/// </summary>
	[HideInEditor]
	[JsonIgnore]
	public int CountCompletionState => CompletionState.Count;

	// [HideInEditor]
	// public int QuestCompletionState { get; set; }

	/// <summary>
	/// Quest journal constructor.
	/// </summary>
	public QuestJournal()
	{
		Title = "!!TITLE!!";
		DisplayName = "!!DESCRIPTION!!";
	}

	/// <summary>
	/// Quest journal constructor.
	/// </summary>
	/// <param name="title"></param>
	/// <param name="displayName"></param>
	public QuestJournal( string title, string displayName )
	{
		Title = title;
		DisplayName = displayName;
	}

	/// <summary>
	/// Add completion state to this quest.
	/// </summary>
	/// <param name="journal"></param>
	/// <param name="state"></param>
	public void AddCompletion( QuestJournal journal, CompletionState state )
	{
		journal.CompletionState.Add( state );
	}

	public CompletionState GetCompletionStateForQuest( int index )
	{
		return CompletionState.ElementAtOrDefault( index );
	}

	public CompletionState GetCompletionStateForQuest( QuestJournal journal, int index )
	{
		return journal.CompletionState.ElementAt( index );
	}

	public void RemoveAt( QuestJournal journal, int index )
	{
		journal.CompletionState.RemoveAt( index );
	}

	public bool CheckCompletionState( int index )
	{
		if (CompletionState.Count <= index)
		{
			Log.Warning("Index is out of bounds!");
		}

		return CompletionState.Count > index;
	}
}