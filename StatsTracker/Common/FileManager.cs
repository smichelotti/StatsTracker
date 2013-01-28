using Newtonsoft.Json;
using StatsTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace StatsTracker.Common
{
    internal static class FileManager
    {
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public async static void SaveGameFileAsync(Game game)
        {
            var gameJson = JsonConvert.SerializeObject(game);
            var fileName = GetGameFileName(game);
            var gameFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(gameFile, gameJson);
        }

        public async static void DeleteGameFileAsync(Game game)
        {
            var fileName = GetGameFileName(game);
            var gameFile = await localFolder.GetFileAsync(fileName);
            await gameFile.DeleteAsync();
        }

        public async static Task<StorageFile> GetGameFileAsync(Game game)
        {
            var fileName = GetGameFileName(game);
            var gameFile = await localFolder.GetFileAsync(fileName);
            return gameFile;
        }

        #region Private Methods

        private static string GetGameFileName(Game game)
        {
            return string.Format("game-{0}-{1}.stgame", game.Opponent, game.Date.ToString("yyyy.MM.dd"));
        }

        #endregion
    }
}
