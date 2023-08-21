using Editor;
using Sandbox;

namespace Vampire;


[HammerEntity]
[Library("item_w_rem_m_700_bach", Title = "Sniper Rifle")]
public partial class Remington : SniperRifle { }

[HammerEntity]
[Library("item_w_ithaca_m_37", Title = "M37 Shotgun")]
public partial class Shotgun : WeaponBase { }

[HammerEntity]
[Library("item_w_glock_17c", Title = "Glock Pistol")]
public partial class Glock : WeaponBase { }