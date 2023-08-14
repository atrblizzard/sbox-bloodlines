using System.Linq;
using Sandbox;
using Bloodlines.UI;
using bloodlines.game.Quest;

namespace Bloodlines.Game.System.Dialog;

public partial class DialogManager : Entity
{
    public static DialogManager Instance { get; private set; }

    [Net] public DialogData Dialog { get; private set; }

    public int CurrentLine
    {
        get
        {
            if (debugMode)
                Log.Info($"Getting value of CurrentLine: {_currentLine}");
            return _currentLine;
        }
        set
        {
            if (debugMode)
                Log.Info($"Setting value of CurrentLine: {value} from {_currentLine}");
            _currentLine = value;
        }
    }

    private bool debugMode;
    private int _currentLine = 1;

    public DialogManager()
    {
        ReadDialogData("vdata/dialog/vv.dialog");

        if (Instance != null) return;
        Instance = this;
    }

    public void CreateInstance()
    {
        if (Instance == null)
        {
            Log.Info("Instance not found, creating one");
            Instance = new DialogManager();
            return;
        }

        Log.Info("Found instance");
    }

    [ConCmd.Server("dialog_debug_mode")]
    public static void DebugMode(bool value)
    {
        Instance.debugMode = value;
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
    public static void DialogPickCmd(int choice)
    {
        if (Instance == null)
        {
            Log.Warning("DialogManager instance is null!");
            return;
        }
        Instance.DialogPick(choice);
    }

    public void DialogPick(int choice)
    {
        if (choice >= 4 && choice <= 0)
        {
            Log.Warning("Out of range!");
            return;
        }
        var entryChoice = choice - 1;
        Log.Info("Trying to pick dialog choice " + choice);
        if (Instance == null)
        {
            Log.Warning("DialogManager instance is null!");
            return;
        }

        if (Instance.Dialog.Entries == null)
        {
            Log.Warning("Dialog entry is null!");
            return;
        }

        var nextLine = Instance.Dialog.Entries.Where(x => x.Id == CurrentLine);

        var npcDialogEntries = nextLine.ToList();

        var entry = npcDialogEntries.First();

        if (entry != null)
        {
            if (entry.Responses.Count <= entryChoice)
            {
                Log.Warning($"Trying to pick dialog choice {choice}, out of bounds.");
                return;
            }
            else
            {
                Log.Info("Trying to pick dialog choice " + choice);
            }

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

            Log.Info($"You picked dialog response choice {choice} with id {response.Id} and link {response.Link}");
            Log.Info($"{response.Text.Values.First()}");

            if (response.Link == 0)
            {
                Log.Info("Dialog ended.");
                Instance.CurrentLine = 0;
                DialogPanel.GetInstance().DialogClose();
                return;
            }

            // Find if link to id exists, if not, reset current line to 0
            // This way we make sure we won't get stuck with any unlinked dialog.
            if (!Instance.Dialog.Entries.Any(x => x.Id == response.Link))
            {
                Log.Error($"Missing link {response.Link} for dialog entries, returning!");
                Instance.CurrentLine = 0;
                DialogPanel.GetInstance().DialogClose();
                return;
            }
            else
            {
                Log.Info(response.Link);
            }

            Instance.CurrentLine = response.Link;
            Instance?.Pick(response.Link);
        }
        else
        {
            Log.Error("Dialog entry npcDialogEntries.First() is null!");
        }


        if (Instance == null) return;
        if (Instance is { CurrentLine: 0 })
        {
            if (debugMode)
                Log.Info("Line returned to 0, closing dialog.");
            //DialogPanel.GetInstance().DialogClose();
            return;
        }
        if (npcDialogEntries.Count == 0)
        {
            Log.Warning($"Couldn't find NPC dialog reply with ID {Instance.CurrentLine}!");
            return;
        }
    }

    [ConCmd.Server("dialog_start")]
    public static void DialogStart(int line)
    {
        if (Instance == null)
        {
            Log.Error("Missing Dialog Manager instance!");
            Instance = new DialogManager();
            return;
        }
        Instance.CurrentLine = line;
        Log.Info($"Set dialog line start to {line}");
        Instance?.Pick(line);
    }

    private void Pick(int line)
    {
        if (Dialog != null)
            DebugLog(line);
    }

    private void DebugLog(int choice)
    {
        if (Dialog.Entries == null)
        {
            Log.Warning("Dialog entry is null!");
            return;
        }

        var nextLine = Dialog.Entries.Where(x => x.Id == CurrentLine);
        var npcDialogEntries = nextLine.ToList();

        if (npcDialogEntries.Count == 0)
        {
            //Log.Warning("npcDialogEntries is empty!");
            return;
        }

        foreach (var entry in npcDialogEntries)
        {
            Log.Info(entry.Id);

            if (!string.IsNullOrEmpty(entry.Text.FirstOrDefault().Value))
            {
                CurrentLine = entry.Id;
                Log.Info($"{entry.Text.FirstOrDefault().Value} - (Line: {CurrentLine})");
                Log.Info("Responses:");
                for (var i = 0; i < entry.Responses.Count; i++)
                {
                    var response = entry.Responses[i];
                    Log.Info($"{i + 1}) {response.Text.FirstOrDefault().Value}");
                    if (debugMode)
                        Log.Info($"(Line {response.Id} / Link: {response.Link})");
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

    public void CheckStartingConditions(int line)
    {
        Log.Info("Checking starting conditions for line: " + line);
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

    public void ReadDialogData(string path)
    {
        var dialog = ResourceLibrary.Get<DialogData>(path);
        if (dialog != null)
        {
            Dialog = dialog;
            Log.Info(Dialog.ResourcePath);
        }
        else
        {
            Log.Info($"Couldn't load dialog data from path {path}!");
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
}