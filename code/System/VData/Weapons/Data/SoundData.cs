using System.Collections.Generic;
using Sandbox;

namespace Vampire.System.VData.Weapons.Data;

public struct SoundData
{
    [ResourceType( "sound" )]
    public Dictionary<SoundType, string> SoundEntry { get; set; }
}