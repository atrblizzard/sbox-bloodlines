﻿using System.Collections.Generic;

namespace Vampire.Data.Dialog;

public interface IDialogEntry
{
	public int Id { get; set; }
	public Dictionary<Gender, string> Text { get; set; }
	public string Condition { get; set; }
	public string Action { get; set; }
	public int Link { get; set; }
}