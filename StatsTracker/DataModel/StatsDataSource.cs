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
            this.NewGameViewModel = new NewGameViewModel();
        }

        private static StatsDataSource dataSource = new StatsDataSource();

        public bool IsProgressVisible { get; set; }
        public GameDetailViewModel SelectedGame { get; set; }
        public NewGameViewModel NewGameViewModel { get; set; }

        public RelayCommand RefreshPlayers { get; private set; }

        private void OnRefreshPlayers()
        {
            RefreshPlayersAsync();
        }

        #region Private Methods

        private async void RefreshPlayersAsync()
        {
            //progressBar.Visibility = Visibility.Visible;
            this.IsProgressVisible = true;
            HttpClient httpClient = new HttpClient();
            var url = string.Format("https://api.teamsnap.com/v2/teams/{0}/as_roster/{1}/rosters", 41132, 1319156);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            //TODO: consider HttpMessageHandler for custom request header
            

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
            //App.ViewModel.AllGroups[1].Items.Clear();
            this.Players.Items.Clear();
            foreach (var item in playersList)
            {
                //App.ViewModel.AllGroups[1].Items.Add(item);
                this.Players.Items.Add(item);
            }


            //progressBar.Visibility = Visibility.Collapsed;
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

            

            // Load Players
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

            // First games (still hard-coded)
            var gameFiles = localFiles.Where(x => x.Name.StartsWith("game-"));
            foreach (var file in gameFiles)
            {
                var json = await FileIO.ReadTextAsync(file);
                var game = JsonConvert.DeserializeObject<Game>(json);
                this.Games.Items.Add(game);
            }

            //var game1 = new Game("Baltimore Alliance", new DateTime(2012, 1, 1), playerStats);
            //game1.Score = "59-27";
            //this.Games.Items.Add(game1);
            //this.Games.Items.Add(new Game("Baltimore Stars", new DateTime(2013, 1, 2), playerStats));
            //this.Games.Items.Add(new Game("Pikesville", new DateTime(2013, 1, 3), playerStats));
            //this.Games.Items.Add(new Game("Ravens A", new DateTime(2013, 1, 4), playerStats));
            //this.Games.Items.Add(new Game("6th Man Warriors", new DateTime(2013, 1, 5), playerStats));
        }
    }
}
