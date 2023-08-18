using System;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vampire.Data.Dialog;

namespace Bloodlines.UI;

public partial class DialogPanel : Panel
{
    public bool IsOpen { get; set; }
    public bool DebugMode { get; set; }
    private bool RequestOpen() => Sandbox.Game.LocalPawn is var _ && Input.Down("score");

    private IDialogEntry hoveredEntry;
    private string _currentDialogLine;

    private List<DialogEntrySlot> dialogSlots = new();

    static VampirePlayer Player => Sandbox.Game.LocalPawn as VampirePlayer;

    public string CurrentDialogLine
    {
        get
        {
            _currentDialogLine = Player.DialogManager?.CurrentLine.ToString();
            return _currentDialogLine;
        }

        set
        {
            _currentDialogLine = value;
            if (int.TryParse(_currentDialogLine, out var intVal))
			{
                if (Player.DialogManager != null)
                {
                    if (DebugMode)
                        Log.Info(_currentDialogLine + " " + intVal);
					Player.DialogManager.CurrentLine = intVal;
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
        else
        {
            Log.Info("Found instance");
            return Instance;
        }
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
        Player.DialogManager.CurrentLine = 1;
        Log.Info("Reset");
        //DialogClose();
    }

    void OnInventorySlotClicked(IDialogEntry entry, int counter)
    {
        var player = Sandbox.Game.LocalPawn as VampirePlayer;
        if (!player.IsValid())
            return;
        
        player.PlaySound("ui.navigate.forward");

		if (player.DialogManager == null)
        {
            Log.Error("player.DialogManager is missing!");
            return;
        }

        player.DialogManager.DialogPick(counter);

		if (IsOpen)
        {
            if (player.DialogManager?.CurrentLine == 0)
            {
                Log.Info("Current line is 0!");
            }
        }

        StateHasChanged();
    }

    List<Option> GetResponseOptions()
    {
        var options = new List<Option>();
        var devices = Player.DialogManager?.Dialog.Entries;
        foreach (var device in devices)
        {
            options.Add(new Option($"{device.Id}) {device.Text.Values.FirstOrDefault()}", device.Id));
        }

        return options;
    }

    List<Option> GetDialogOptions()
    {
        var options = new List<Option>();
        var devices = Player.DialogManager?.Dialog.Entries;
        foreach (var device in devices)
        {
            options.Add(new Option($"{device.Id}) {device.Text.Values.FirstOrDefault()}", device.Id));
        }

        return options;
    }

    public override void Tick()
    {
        base.Tick();

        if (!IsOpen) return;

        if (Input.Pressed("Slot1"))
            Player.DialogManager.DialogPick(1);

        if (Input.Pressed("Slot2"))
            Player.DialogManager.DialogPick(2);

        if (Input.Pressed("Slot3"))
            Player.DialogManager.DialogPick(3);

        if (Input.Pressed("Slot4"))
            Player.DialogManager.DialogPick(4);
    }

    public void DialogOpen()
    {
        //DebugMode = Player.DialogManager.DebugMode;
        Log.Info(DebugMode);
        if (Player.DialogManager != null)
        {
            if (DebugMode)
                Log.Info(Player.DialogManager?.CurrentLine);
        }
        else
        {
            Log.Error("DialogManager.Instance is missing!");
        }

        IsOpen = true;
    }

    [ConCmd.Client("dialog_open")]
    public static void Command_OpenDialogPanel()
    {
		if (Instance != null)
        {
            if (Player.DialogManager != null)
            {
                Log.Info(Player.DialogManager?.CurrentLine);
                Player.DialogManager.CurrentLine = 1;
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

    protected override int BuildHash()
    {
        return HashCode.Combine(IsOpen, Player.DialogManager?.GetCurrentLine(), Player.DialogManager?.GetActiveDialog());
    }
}