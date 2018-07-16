using Plukit.Base;

namespace NimbusFox.Minigame.Interfaces.V1 {
    public interface IGameMode {
        IGameType GameType { get; }
        string Code { get; }
        void Init(Blob config);
    }
}