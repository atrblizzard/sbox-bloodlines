using Sandbox;
using Vampire.System.VData.Items.Data;

namespace Vampire.System.VData.Items;

[GameResource("Item Type Data", "itemtype", "VTM:B Item Type Definitions")]
public class ItemTypeData : GameResource
{
    public InventorySections InventorySections { get; set; }
    public ItemTypes ItemTypes { get; set; }
    public BotchTypes BotchTypes { get; set; }
    public BotchTables BotchTables { get; set; }
    public ActivationTypes ActivationTypes { get; set; }
    public StartingEquipTables StartingEquipTables { get; set; }
    public ExcludedEquipTables ExcludedEquipTables { get; set; }
    public MPEquipTables MPEquipTables { get; set; }
}