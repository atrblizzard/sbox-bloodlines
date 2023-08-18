using System.Linq;
using Sandbox;
using Bloodlines.UI;
using System.Collections.Generic;
using Vampire;
using Vampire.Data.Dialog;

namespace Bloodlines.Systems.Dialog;

public partial class DialogManager : EntityComponent
{
    public static DialogManager Instance { get; private set; }

    [Net] public DialogData Dialog { get; private set; }

    //[Net, Predicted] 
    public int CurrentLine { get; set; }

	[Net, Predicted] public bool DebugMode { get; set; }

    private void ShowLog(string message)
    {
        if (DebugMode)
            Log.Info(message);
    }
    
    public DialogManager()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void CreateInstance()
    {
        if (Instance == null)
        {
            ShowLog("Instance not found, creating one");
            Instance = new DialogManager();
            return;
        }

        ShowLog("Found instance");
    }

    [ConCmd.Server("dialog_debug_mode")]
    public static void Cmd_SetDebugMode(bool value)
    {
        Instance.DebugMode = value;
        Cl_SetDebugMode(value);
    }

    [ConCmd.Client("cl_dialog_debug_mode")]
    public static void Cl_SetDebugMode(bool value)
    {
        Instance.DebugMode = value;
        Log.Info($"Setting client debug value: {value}");
    }

    /// <summary>
    /// Loads dialog data from a file.
    /// </summary>
    /// <param name="path"></param>
    [ConCmd.Server("dialogload")]
    public static void LoadDialogData(string path)
    {
        if (Instance == null)
        {
            Log.Warning("DialogManager instance is null!");
            return;
        }
        Instance.ReadDialogData(path);
    }

    /// <summary>
    /// Activates (picks) a dialog line.
    /// </summary>
    /// <param name="choice"></param>
    [ConCmd.Server("dialogpick")]
    public static void Cmd_DialogPick(int choice)
    {
        if (Instance == null)
        {
            Log.Warning("DialogManager instance is null!");
            return;
        }

        if (choice > 4)
        {
            Log.Warning("Out of range!");
            return;
        }

        Instance.DialogPick(choice);
    }

    public void DialogPick(int choice)
    {
        var entryChoice = choice - 1;

        ShowLog($"Trying to pick dialog choice {choice}");

        if (Dialog == null)
        {
            Log.Warning("Instance.Dialog is null!");
            return;
        }

        if (Dialog.Entries == null)
        {
            Log.Warning("Dialog entry is null!");
            return;
        }

        var nextLine = Dialog.Entries.Where(x => x.Id == CurrentLine);

        var npcDialogEntries = nextLine.ToList();

        var entry = npcDialogEntries.First();

        if (entry != null)
        {
            if (entry.Responses.Count <= entryChoice)
            {
                Log.Warning($"Trying to pick dialog choice {choice}, out of bounds.");
                return;
            }

            ShowLog($"Trying to pick dialog response choice {choice}");

            if (entry.Responses == null)
            {
                Log.Error($"Responses are null for some reason! Bad!");
                return;
            }

            if (entry.Responses.Count == 0)
            {
                Log.Warning($"Couldn't find NPC dialog reply with ID {entryChoice}!");
                return;
            }

            var response = entry.Responses.ElementAt(entryChoice);
            if (response == null)
            {
                Log.Warning($"Couldn't find player dialog reply with valid ID!");
                return;
            }
            
            ShowLog($"You picked dialog response choice {choice} with id {response.Id} and link {response.Link}");

            if (response.Link == 0)
            {
                ShowLog("Dialog ended.");
                CurrentLine = 0;
                DialogPanel.GetInstance()?.DialogClose();
                return;
            }

            // Find if link to id exists, if not, reset current line to 0
            // This way we make sure we won't get stuck with any unlinked dialog.
            if (!Dialog.Entries.Any(x => x.Id == response.Link))
            {
                Log.Error($"Missing link {response.Link} for dialog entries, returning!");
                CurrentLine = 0;
                DialogPanel.GetInstance()?.DialogClose();
                return;
            }

            CurrentLine = response.Link;
            Pick(response.Link);
        }
        else
        {
            Log.Error("Dialog entry npcDialogEntries.First() is null!");
        }


        if (CurrentLine == 0)
        {
            ShowLog("Line returned to 0, closing dialog.");
            //DialogPanel.GetInstance().DialogClose();
            return;
        }

        if (npcDialogEntries.Count == 0)
        {
            Log.Warning($"Couldn't find NPC dialog reply with ID {CurrentLine}!");
        }
    }

    [ConCmd.Server("dialog_start")]
    public static void Cmd_DialogStart(int line)
    {
        if (Instance.Dialog == null)
		    Instance.ReadDialogData("vdata/dialog/vv.dialog");

		if (Instance == null)
        {
            Log.Error("Missing Dialog Manager instance!");
            Instance = new DialogManager();
            return;
        }
        Instance.CurrentLine = line;
        Instance.ShowLog($"Set dialog line start to {line}");
        Instance.Pick(line);
    }

    private void Pick(int line)
    {
        if (Sandbox.Game.IsClient)
            CurrentLine = line;
        
        ShowLog("Picked line " + CurrentLine);

        if (Dialog == null)
        {
            Log.Warning("Dialog is null!");
            return;
        }

		if (Dialog.Entries == null)
		{
			Log.Warning("Dialog entries are null!");
			return;
		}

        var nextLine = Dialog.Entries.Where(x => x.Id == CurrentLine);
		var npcDialogEntries = nextLine.ToList();

		if (npcDialogEntries.Count == 0)
		{
			//Log.Warning("npcDialogEntries is empty!");
			return;
		}

		foreach (var entry in npcDialogEntries.Where(entry => !string.IsNullOrEmpty(entry.Text.FirstOrDefault().Value)))
		{
			CurrentLine = entry.Id;
        }

        DebugLog(npcDialogEntries);
    }

    private void DebugLog(List<NPCDialogEntry> npcDialogEntries)
    {
        if (DebugMode == false) return;

        ShowLog($"Picked current NPC line {CurrentLine}!");
        foreach (var entry in npcDialogEntries)
        {
            ShowLog($"Entry ID: {entry.Id}");

            if (!string.IsNullOrEmpty(entry.Text.FirstOrDefault().Value))
            {
                ShowLog($"{entry.Text.FirstOrDefault().Value} - (Line: {CurrentLine})");
                ShowLog("Responses:");
                for (var i = 0; i < entry.Responses.Count; i++)
                {
                    var response = entry.Responses[i];
                    ShowLog($"{i + 1}) {response.Text.FirstOrDefault().Value}");
                    ShowLog($"(Line {response.Id} / Link: {response.Link})");
                }
            }
        }
    }

    public void CallEventScript()
    {

    }

    public void PlayWhisper()
    {

    }

    public void Acquire()
    {

    }

    public void GetStartingLine()
    {

    }

    public int GetCurrentLine()
    {
        return CurrentLine;
    }

    public void SetDialogWindowActive()
    {

    }

    public bool CheckStartingConditionsButtons(int line)
    {        
        if (Dialog == null)
        {
            Log.Error("Dialog is null!");
            return false;
        }

        if (Sandbox.Game.LocalPawn is not VampirePlayer player)
            return false;

        var responseEntry = GetActiveDialog().Responses.FirstOrDefault(x => x.Id == line);        
        ConditionParser.IsDebugMode = DebugMode;
        if (responseEntry != null)
        {
            var parsedValues = ConditionParser.ParseCondition(responseEntry.Condition);
            
            ShowLog($"Response entry count for {responseEntry.Id}: {responseEntry.Condition.Length}");
            ShowLog($"Parsed values count: {parsedValues.Count}");
            
            // TODO: for now we have a simpler parser just to demonstrate its functionality.
            // The player can store a state and check if it's true or false, similar to the G. states in Python.
            // It can also check the attributes of the player and clan (albeit in a much simpler version).
            // It can also currently check the not operator and if the argument is numeric for attributes.
            // A lot of the stuff here should be moved to the parser itself, but for now it's just a proof of concept.

            foreach (var kvp in parsedValues)
            {
                if (kvp.Value is List<string> arguments)
                {
                    foreach (var argument in arguments)
                    {
                        if (arguments.Contains("not"))
                        {
                            // We have hardcoded "pc" for "IsClan" for the sake of demonstration.
                            // Here we check if the player is not a member of the specified clan.
                            if (arguments[1] == "pc" && arguments[2] != player.Clan)
                            {
                                ShowLog("Found not match!");
                                return true;
                            }
                            return false;
                        }

                        if (!arguments.Contains("not"))
                        {
                            // Ditto, except here we check if the player is a member of the specified clan.
                            if (arguments[0] == "pc" && arguments[1] == player.Clan)
                            {
                                ShowLog("Found match!");
                                return true;
                            }

                            // Here we check if the argument is numeric, for example the player's attributes.
                            var isNumeric = int.TryParse(arguments[1], out var n);
                            if (isNumeric)
                            {
                                ShowLog("Argument 1 is numeric:" + n);
                                if (player.GetAttribute(arguments[0]) == (object)n)
                                {
                                    ShowLog("Found match!");
                                    return true;
                                }
                            }

                            return false;
                        }

                        ShowLog($"Didn't find match for {argument}!");

                        return false;
                    }
                }

                //ShowLog(kvp.Value.ToString());
                
                ShowLog("Checking starting conditions for line: " + line);
                ShowLog(kvp.Key + ": " + kvp.Value);
                
                ShowLog($"{kvp.Value}: {player.GetAttribute(kvp.Key)}");
                if (player.GetAttribute(kvp.Key) == kvp.Value)
                {
                    ShowLog("Found match!");
                    return true;
                }

                if (responseEntry.Condition.Contains(kvp.Key) && kvp.Value.ToString() == 
                    GlobalStates.Instance.GetState(kvp.Key).ToString())
                {
                    ShowLog($"Found matching stuff: {kvp.Key}: {kvp.Value}");
                    return true;
                }

                return false;
            }
        }

        return true;
    }

    public void ShowHistoryWindow()
    {

    }

    public void ShowPlayerChoices()
    {

    }

    public void LookupSpeechFile()
    {

    }

    public void NPCNotifyDoneTalking()
    {

    }

	[ClientRpc]
	public void DialogReadData(DialogData dialog)
    {
        if (Dialog != null)
        {
            if (Dialog.Entries != null)
            {
                if (Dialog.ResourceName == dialog.ResourceName)
                {
                    return;
                }

				Dialog = dialog;
                ShowLog($"Read dialog counts: {Dialog.Entries.Count}");
            }
            else
                Log.Error("No dialog entries have been found!");
        }
		else
		{
            ShowLog("Dialog returned null, this is not good!");
		}
	}

    public void ReadDialogData(string path)
    {
		if (!string.IsNullOrWhiteSpace(path))
		{
			if (ResourceLibrary.TryGet<DialogData>(path, out var dialog))
			{
                ShowLog($"Found dialog at path {path}");
				Dialog = dialog;
				DialogReadData(dialog);

				if (Dialog != null)
                {
                    if (Dialog.Entries == null)
                        Log.Error("No dialog entries have been found!");
                    else
                        ShowLog($"Read dialog counts: {Dialog.Entries.Count}");
                }
                else
                {
                    ShowLog("Dialog returned null, this is not good!");
                }
			}
		}
    }

    public DialogData GetDialog()
    {
        return Dialog;
    }

    public NPCDialogEntry GetActiveDialog()
    {
        return Dialog?.Entries.FirstOrDefault(x => x.Id == CurrentLine);
    }

    [ClientRpc]
    public void ShowDialog()
    {
        ShowLog("Trying to show dialog!");
        if (Sandbox.Game.IsClient)
        {
            CurrentLine = 1;
            var dialogPanel = DialogPanel.CreateInstance();
            dialogPanel.DebugMode = DebugMode;
            dialogPanel.DialogOpen();
        }
        else
        {
            if (DialogPanel.GetInstance() != null)
            {
                DialogPanel.GetInstance().DialogOpen();
            }
            else
            {
                Log.Error("DialogPanel.GetInstance() is missing!");
            }
        }
    }

	internal void ClearDialog()
	{
        Dialog = null;
	}
}