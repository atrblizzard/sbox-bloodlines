using Sandbox;
using System;

namespace Vampire.System.VData.Weapons.Data;

public struct SoundEntry
{
    public SoundType SoundType { get; set; }
    [ResourceType( "sound" )]
    public string Sound { get; set; }
}