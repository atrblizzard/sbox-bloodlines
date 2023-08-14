using System;
using Sandbox;
using Sandbox.UI;
using Bloodlines.Game.System.Dialog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bloodlines.UI;

public partial class DialogPanel : Panel
{
    public bool IsOpen { get; set; }
    public bool DebugMode { get; set; }
    private bool RequestOpen() => Sandbox.Game.LocalPawn is var _ && Input.Down("score");
    private IDialogEntry _hoveredWeapon;
    private string _currentDialogLine;

    public string CurrentDialogLine
    {
        get
        {
            _currentDialogLine = DialogManager.Instance?.CurrentLine.ToString();
            return _currentDialogLine;
        }

        set
        {
            _currentDialogLine = value;
            if (int.TryParse(_currentDialogLine, out var intVal))
            {
                if (DialogManager.Instance != null)
                {
                    if (DebugMode)
                        Log.Info(_currentDialogLine + " " + intVal);
                    DialogManager.Instance.CurrentLine = intVal;
                    StateHasChanged();
                }
            }
        }
    }

    private static DialogPanel Instance { get; set; }

    public static DialogPanel CreateInstance()
    {
	    if (Instance == null)
        {
			Log.Info("Instance not found, creating one");
			return new DialogPanel();
        }

	    Log.Info("Found instance");
	    return Instance;
    }

    public static DialogPanel GetInstance() { return Instance; }

    public DialogPanel()
    {
	    Instance = this;
    }

	[ConCmd.Client("dialog_reset")]
	public static void Cmd_ResetDialog()
    {
        if (!Instance.DebugMode) return;

        if (Instance != null)
        {
            Instance.OnResetDialog();
        }
        else
        {
            CreateInstance();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        StateHasChanged();
    }

    private Task UpdateList()
    {
        throw new NotImplementedException();
    }

    void OnResetDialog()
    {
        DialogManager.Instance.CurrentLine = 1;
        Log.Info("Reset");
        //DialogClose();
    }

    void OnInventorySlotClicked(IDialogEntry entry, int counter)
    {
        var player = Sandbox.Game.LocalPawn as Player;
        if (!player.IsValid())
            return;
        
        player.PlaySound("ui.navigate.forward");

		if (DialogManager.Instance == null)
        {
            Log.Error("DialogManager.Instance is missing!");
            return;
        }

        DialogManager.Instance.DialogPick(counter);


        if (IsOpen)
        {
            if (DialogManager.Instance?.CurrentLine == 0)
            {
                Log.Info("Current line is 0!");
                //DialogManager.CurrentLine = 1;
                //DialogClose();
            }
        }

        StateHasChanged();
    }

    List<Option> GetResponseOptions()
    {
        var options = new List<Option>();
        var devices = DialogManager.Instance?.Dialog.Entries;
        foreach (var device in devices)
        {
            options.Add(new Option(device.Id + ") " + device.Text.Values.FirstOrDefault(), device.Id));
        }

        return options;
    }

    List<Option> GetDialogOptions()
    {
        var options = new List<Option>();
        var devices = DialogManager.Instance?.Dialog.Entries;
        foreach (var device in devices)
        {
            options.Add(new Option(device.Id + ") " + device.Text.Values.FirstOrDefault(), device.Id));
        }

        return options;
    }

    protected override int BuildHash()
    {
        return HashCode.Combine(IsOpen, base.BuildHash(), DialogManager.Instance?.GetCurrentLine(), DialogManager.Instance?.GetActiveDialog()?.Responses.Count);
    }

    public override void Tick()
    {
        base.Tick();

        if (!IsOpen) return;

        if (Input.Pressed("Slot1"))
            DialogManager.Instance.DialogPick(1);

        if (Input.Pressed("Slot2"))
            DialogManager.Instance.DialogPick(2);

        if (Input.Pressed("Slot3"))
            DialogManager.Instance.DialogPick(3);

        if (Input.Pressed("Slot4"))
            DialogManager.Instance.DialogPick(4);
    }

    [ConCmd.Client("dialog_open")]
    public static void Command_OpenDialogPanel()
    {
		if (Instance != null)
        {
            if (DialogManager.Instance != null)
            {
                Log.Info(DialogManager.Instance.CurrentLine);
                DialogManager.Instance.CurrentLine = 1;
            }
            else
            {
                Log.Error("DialogManager.Instance is missing!");
            }

			Instance.IsOpen = true;
		}
        else
        {
            Log.Error("Instance is null for dialogpanel!");
            Instance = CreateInstance();
        }
    }

    [ConCmd.Client("dialog_force_close")]
    public static void Command_CloseDialogPanel()
    {
        Instance.DialogClose();
    }

    public void DialogClose()
    {       
        IsOpen = false;
    }


	[ConCmd.Client("dialog_toggle")]
	static void Command_ToggleDialog()
	{
        if (Instance == null)
        {
            Log.Error("Instance is null!");
        }
		Instance?.Toggle();
	}

    public void Toggle()
    {
		IsOpen = !IsOpen;
	}
}