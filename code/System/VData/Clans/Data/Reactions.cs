using System.Collections.Generic;

namespace Vampire.Data.Clans;

public struct Reactions
{
    public Dictionary<string, int> To { get; set; }
    public Dictionary<string, int> From { get; set; }
}