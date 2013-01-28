using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using StatsTracker.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace StatsTracker.Data
{
    public class NewGameViewModel : ViewModelBase
    {
        public NewGameViewModel()
        {
            this.Game = new Game();
            this.OpenAddGame = new RelayCommand(() => OnOpenAddGame());
            this.SaveNewGame = new RelayCommand(() => OnSaveNewGame());
        }

        public RelayCommand OpenAddGame { get; private set; }
        public RelayCommand SaveNewGame { get; private set; }

        public bool AddGameDialogIsVisible { get; set; }
        public Game Game { get; private set; }


        private void OnOpenAddGame()
        {
            this.Game = new Game();
            this.AddGameDialogIsVisible = true;
        }

        private void OnSaveNewGame()
        {
            var playerStats = App.ViewModel.Players.Items.Cast<Player>().Select(p => new PlayerStat(p));
            //this.Game.PlayerStats = new ObservableCollection<PlayerStat>(playerStats);
            App.ViewModel.Games.Items.Add(this.Game);
            FileManager.SaveGameFileAsync(this.Game);

            this.AddGameDialogIsVisible = false;
        }
    }
}
