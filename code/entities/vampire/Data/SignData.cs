using Sandbox;
using System.Collections.Generic;

namespace bloodlines.entities.vampire.Data
{
	public class BackgroundImage
	{
		[Property]
		public string Name { get; set; }
		public string XPos { get; set; }
		public string YPos { get; set; }
		public string Wide { get; set; }
		public string Tall { get; set; }
	}

	public class Label
	{
		[Property]
		public string Text { get; set; }
		public string Alignment { get; set; }
		public string Font { get; set; }
		public string XPos { get; set; }
		public string YPos { get; set; }
		public string Wide { get; set; }
		public string Tall { get; set; }
		public string TextRGBA { get; set; }
		public string BackgroundRGBA { get; set; }
		public string Image { get; set; }
	}

	public class TextBlock
	{
		[Property]
		public string Text { get; set; }
		public string Font { get; set; }
		public string XPos { get; set; }
		public string YPos { get; set; }
		public string Wide { get; set; }
		public string Tall { get; set; }
		public string TextRGBA { get; set; }
		public string BackgroundRGBA { get; set; }
	}

	[Library( "signdata" ), AutoGenerate]
	public sealed partial class SignData // : AssetType
	{
		[Property]
		public BackgroundImage BackgroundImage { get; set; }
		[Property]
		public List<Label> Label { get; set; }
		[Property]
		public List<TextBlock> TextBlock { get; set; }
	}
}
