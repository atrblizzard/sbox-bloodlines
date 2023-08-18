using Sandbox;
using System.Collections.Generic;

namespace Vampire.System.Vendor;

[GameResource("VendorData", "vendor", "VTM:B Vendors Definitions")]
public class VendorData : GameResource
{
    public List<Data.Vendor> Vendors { get; set; }
}