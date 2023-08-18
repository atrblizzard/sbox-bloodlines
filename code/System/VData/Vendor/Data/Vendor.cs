using System.Collections.Generic;

namespace Vampire.System.Vendor.Data;

public struct Vendor
{
	public string Name { get; set; }
	public string InternalName { get; set; }
	public string VendorImage { get; set; }
	public List<VendorItem> VendorItems { get; set; }
}