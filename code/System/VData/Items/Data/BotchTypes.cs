
using System.Collections.Generic;

namespace Vampire.System.VData.Items.Data;

public struct BotchTypes
{
    public string Name { get; set; }
    public string InternalName { get; set; }

    public List<BotchType> BotchType { get; set; }
}