using System.Collections.Generic;

namespace Vampire.System.Clans.Data;

public struct Reactions
{
    public Dictionary<string, int> To { get; set; }
    public Dictionary<string, int> From { get; set; }
}