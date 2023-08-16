using Bloodlines.Entities.Vampire;
using Editor;

namespace Sandbox.Entities.Vampire.Level;

[ClassName( "inspection_node" ), Description("Inspection Node")]
[HammerEntity]
[EditorSprite( "materials/editor/inspectionnode.vmat" )]
[Title( "Inspection Node" ), Category( "Nodes" ), Icon( "select_all" )]
public partial class InspectionNode : VEntity
{
	/// <summary>
	/// Required inspection level (0-10)
	/// </summary>
	[Property ("inspection", Title = "Inspection Level")]
	public int Inspection { get; set; }
}

