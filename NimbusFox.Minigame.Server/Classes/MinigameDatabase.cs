using System;
using System.Collections.Generic;
using System.Linq;
using NimbusFox.FoxCore.Managers;
using NimbusFox.Minigame.Interfaces.V1;
using Plukit.Base;

namespace NimbusFox.Minigame.Server.Classes {
    public class MinigameDatabase {
        private static readonly Dictionary<string, IGameType> GameTypes = new Dictionary<string, IGameType>();

        internal void Init() {
            var loadConfigs = new Dictionary<string, Action<Blob>>();

            foreach (var gameType in GameTypes.Values) {
                foreach (var type in gameType.ExtensionBlobs) {
                    if (loadConfigs.ContainsKey(type.Key)) {
                        Logger.WriteLine("Unable to assign " + type.Key + " to " + gameType.Name);
                        continue;
                    }
                    loadConfigs.Add(type.Key, type.Value);
                }
            }

            if (loadConfigs.Any()) {
                CycleDirectories(MinigameServer.FoxCore.ModsDirectory, loadConfigs);
            }
        }

        private void CycleDirectories(DirectoryManager currentDirectory, Dictionary<string, Action<Blob>> extensionFunctions) {
            foreach (var dir in currentDirectory.Directories) {
                CycleDirectories(currentDirectory.FetchDirectory(dir), extensionFunctions);
            }

            foreach (var file in currentDirectory.Files.Where(x => extensionFunctions.Any(y => string.Equals(y.Key, x.Split('.').Last(), StringComparison.CurrentCultureIgnoreCase)) )) {
                var fileTexts = file.Split('.');

                if (!fileTexts.Any()) {
                    continue;
                }

                foreach (var extension in extensionFunctions) {
                    if (fileTexts.Any(x => string.Equals(x, extension.Key, StringComparison.CurrentCultureIgnoreCase))) {
                        currentDirectory.ReadFile(file, extension.Value, true);
                    }
                }
            }
        }

        public static void AddGameMode(string code, IGameType gameTypeInfo) {
            if (GameTypes.ContainsKey(code)) {
                throw new Exception("Unable to assign " + code + " to GameType " + gameTypeInfo.Name);
            }
            GameTypes.Add(code, gameTypeInfo);
        }
    }
}
