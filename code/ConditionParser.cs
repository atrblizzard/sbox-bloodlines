using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vampire;

public static class ConditionParser
{
    public static bool IsDebugMode { get; set; }

	public static Dictionary<string, object> ParseCondition(string condition)
	{
        Dictionary<string, object> variables = new();

        // Regular expression to match patterns like "G.VariableName == 5"
        Regex regex = new Regex(@"G\.(\w+)\s*([=><!]+)\s*(\d+)");
		foreach (Match match in regex.Matches(condition))
		{
			string variableName = match.Groups[1].Value;
			int value = int.Parse(match.Groups[3].Value);
			variables[variableName] = value;
		}

        // 2. Parse Attributes
        var attributeRegex = new Regex(@"(\w+)\s+(\d+)");
        foreach (Match match in attributeRegex.Matches(condition))
        {
            string attributeName = match.Groups[1].Value;
            int value = int.Parse(match.Groups[2].Value);
            variables[attributeName] = value;
        }

        // 3. Parse Functions with optional 'not' keyword
        var functionWithNotRegex = new Regex(@"(not\s+)?(\w+)\(([^)]+)\)");
        foreach (Match match in functionWithNotRegex.Matches(condition))
        {
            string isNot = match.Groups[1].Value.Trim();
            string functionName = match.Groups[2].Value;
            // For now, store the arguments as a list of strings
            var args = new List<string>(match.Groups[3].Value.Split(','));
            for (int i = 0; i < args.Count; i++)
            {
                args[i] = args[i].Trim().Replace("\"", "");
                if (IsDebugMode)
                    Log.Info($"Args: {args[i]}");
            }            
            if (!string.IsNullOrEmpty(isNot))
            {
                if (IsDebugMode)
                    Log.Info($"Found \"not\" operator for argument {args}!");
                args.Insert(0, isNot);
                
            }
            variables[functionName] = args;
        }

        return variables;
	}
}