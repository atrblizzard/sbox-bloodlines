using System.Collections.Generic;
using Vampire.System.Character.Data;

namespace Vampire.System.Stats;

public struct Stat
{
	public string Name { get; set; }
	public string InternalName { get; set; }
	public string Description { get; set; }
	public HelpTextData HelpText { get; set; }

	public bool ShownInFeedback { get; set; }
	public bool IsRenewable { get; set; }
	public bool IsPassive { get; set; }
	public bool IsInstant { get; set; }
	public bool IsAggressive { get; set; }
	public bool ForceOrdering { get; set; }

	public string Min { get; set; }
	public string Max { get; set; }
	public string Default { get; set; }
	public string NameMapping { get; set; }

	public Costs Costs { get; set; }

	public List<Table> Table { get; set; }
}

// ActiveDisciplines