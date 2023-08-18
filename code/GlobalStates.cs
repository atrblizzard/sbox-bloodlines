using System.Collections.Generic;
using Sandbox;

namespace Vampire;

public partial class GlobalStates : BaseNetworkable
{
	// Singleton pattern
	private static GlobalStates _instance;
	public static GlobalStates Instance => _instance ??= new GlobalStates();

	//[Net, Predicted]
	public IDictionary<string, int> States { get; set; } = new Dictionary<string, int>();

	private GlobalStates() { } // Private constructor to prevent external instantiation

	public void SetState(string key, int value)
	{
		States[key] = value;
	}

	/// <summary>
	/// Sets a global state from the console
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	[ConCmd.Server("setstate")]
	public static void Cmd_SetState(string key, int value)
	{
		Instance.SetState(key, value);
		Cmd_ClSetState(key, value);
	}
	
	[ClientRpc]
	public static void Cmd_ClSetState(string key, int value)
	{
		Instance.SetState(key, value);
	}
	
	[ConCmd.Server("getstate")]
	public static void Cmd_GetState(string key)
	{		
        Log.Info(Instance.GetState(key));
		Cmd_ClGetState(key);
	}

    [ClientRpc]
    public static void Cmd_ClGetState(string key)
    {
        Log.Info(Instance.States.TryGetValue(key, out var value) ? value : 0);
        Log.Info(Instance.GetState(key));
    }
  
    public int GetState(string key)
	{
		return States.TryGetValue(key, out var value) ? value : 0; // Return 0 if not found; adjust as needed
	}

	public bool EvaluateCondition(string condition)
	{
		// TODO: implement condition parsing and evaluation logic here
		return false;
	}
}