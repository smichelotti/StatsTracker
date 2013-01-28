using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using StatsTracker.Common;
using StatsTracker.Data;
using StatsTracker.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StatsTracker.Data
{
    public class StatsDataSource : ViewModelBase
    {
        public StatsDataSource()
        {
            var gamesGroup = new AppDataGroup("Games");
            var playersGroup = new AppDataGroup("Players");

            this.AllGroups.Add(gamesGroup);
            this.allGroups.Add(playersGroup);

            this.RefreshPlayers = new RelayCommand(() => OnRefreshPlayers());
            this.ImportGame = new RelayCommand(() => OnImportGameAsync());
            this.NewGameViewModel = new NewGameViewModel();
        }

        private static StatsDataSource dataSource = new StatsDataSource();

        public bool IsProgressVisible { get; set; }
        public GameDetailViewModel SelectedGame { get; set; }
        public NewGameViewModel NewGameViewModel { get; set; }

        public RelayCommand RefreshPlayers { get; private set; }
        public RelayCommand ImportGame { get; private set; }

        private void OnRefreshPlayers()
        {
            RefreshPlayersAsync();
        }

        private async void OnImportGameAsync()
        {
            var openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            openPicker.FileTypeFilter.Add(".stgame");
            StorageFile file = await openPicker.PickSingleFileAsync();
            var game = await file.ToGameAsync();
            FileManager.SaveGameFileAsync(game);
            App.ViewModel.Games.Items.Add(game);
        }

        #region Private Methods

        private async void RefreshPlayersAsync()
        {
            this.IsProgressVisible = true;
            HttpClient httpClient = new HttpClient();
            var url = string.Format("https://api.teamsnap.com/v2/teams/{0}/as_roster/{1}/rosters", 41132, 1319156);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            //TODO: consider HttpMessageHandler for custom request header
            // X-Teamsnap-Token here:
            

            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var rosterRoot = JsonConvert.DeserializeObject<List<RosterRoot>>(json);
            var rosters = rosterRoot.Where(x => x.roster.non_player == false).Select(x => x.roster).ToList();
            var data = new ObservableCollection<Roster>(rosters.ToList());

            var localFolder = ApplicationData.Current.LocalFolder;

            var rosterFile = await localFolder.CreateFileAsync("rosterData", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(rosterFile, json);

            // first need to locally store all player images
            try
            {
                var httpClient2 = new HttpClient();
                httpClient2.MaxResponseContentBufferSize = 256000;

                var localFiles = await localFolder.GetFilesAsync();
                foreach (var item in rosters)
                {
                    var fileName = item.GetImageFileName();

                    if (!localFiles.Any(x => x.Name == fileName))
                    {
                        var imgResponse = await httpClient2.GetAsync(item.full_picture_url);
                        var newImgFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                        var buffer = await imgResponse.Content.ReadAsByteArrayAsync();
                        await FileIO.WriteBytesAsync(newImgFile, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var playersList = rosters.Select(x => new Player(x.first + " " + x.last, x.number, x.GetImageFileName())).OrderBy(x => x.Number);
            this.Players.Items.Clear();
            foreach (var item in playersList)
            {
                this.Players.Items.Add(item);
            }

            this.IsProgressVisible = false;
        }

        #endregion

        private ObservableCollection<AppDataGroup> allGroups = new ObservableCollection<AppDataGroup>();
        public ObservableCollection<AppDataGroup> AllGroups
        {
            get { return this.allGroups; }
        }

        public AppDataGroup Games
        {
            get
            {
                return this.AllGroups[0];
            }
        }

        public AppDataGroup Players
        {
            get
            {
                return this.AllGroups[1];
            }
        }

        public async void InitialilzeFromStorageAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            // First: Load Players
            var localFiles = await localFolder.GetFilesAsync();
            if (localFiles.Any(x => x.Name == "rosterData"))
            {
                var rosterFile = await localFolder.GetFileAsync("rosterData");
                var json = await FileIO.ReadTextAsync(rosterFile);
                var rosterRoot = JsonConvert.DeserializeObject<List<RosterRoot>>(json);
                var rosters = rosterRoot.Where(x => x.roster.non_player == false).Select(x => x.roster).ToList();
                var playersList = rosters.Select(x => new Player(x.first + " " + x.last, x.number, x.GetImageFileName())).OrderBy(x => x.Number);
                this.Players.Items.Clear();
                foreach (var item in playersList)
                {
                    this.Players.Items.Add(item);
                }
            }

            var playerStats = this.Players.Items.Cast<Player>().Select(p => new PlayerStat(p));

            // Second: Load Games
            var gameFiles = localFiles.Where(x => x.Name.StartsWith("game-"));
            foreach (var file in gameFiles)
            {
                var game = await file.ToGameAsync();
                this.Games.Items.Add(game);
            }
        }
    }
}
