using Editor;

namespace Sandbox.Entities.Vampire.Level.Items;

[ClassName("item_m_money_envelope")]
[HammerEntity]
public class MoneyEnvelopeItem : ItemBase
{
	[Property( "starting_money", Title = "Starting Money" )]
	public int StartingMoney { get; set; }
}
