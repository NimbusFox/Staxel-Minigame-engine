using System;
using System.Collections.Generic;
using Plukit.Base;

namespace NimbusFox.Minigame.Interfaces.V1 {
    public interface IGameType {
        string Name { get; }
        string Code { get; }
        Dictionary<string, Action<Blob>> ExtensionBlobs { get; }
    }
}