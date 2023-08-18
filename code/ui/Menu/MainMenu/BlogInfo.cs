using System.Text.Json.Serialization;

namespace Bloodlines.Menu;

public class BlogInfo
{
	public string Name { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string URL { get; set; }
	[JsonPropertyName( "thumb" )] public string Thumbnail { get; set; }
}