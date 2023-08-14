using Sandbox;
using System.Collections.Generic;
using Vampire.System.VData.News.Data;

namespace Vampire.System.VData.News;

[GameResource("News Data", "news", "VTM:B News Definitions")]
public class NewsData : GameResource
{
    public List<Story> Story;
}