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
    public class EditGameViewModel : ViewModelBase
    {
        public EditGameViewModel()
        {
            this.Game = new Game();
            this.OpenAddGame = new RelayCommand(() => OnOpenAddGame());
            this.SaveGame = new RelayCommand(() => OnSaveGame());
        }

        public RelayCommand OpenAddGame { get; private set; }
        public RelayCommand SaveGame { get; private set; }

        public bool AddGameDialogIsVisible { get; set; }
        public Game Game { get; private set; }
        public bool IsNewGame { get; private set; }

        public bool IsExistingGame
        {
            get
            {
                return !this.IsNewGame;
            }
        }
        
        public string PageTitle
        {
            get
            {
                return (this.IsNewGame ? "Add New Game" : "Edit Game");
            }
        }

        private void OnOpenAddGame()
        {
            this.IsNewGame = true;
            this.Game = new Game();
            Navigator.NavigateTo<EditGamePage>();
        }

        public void OpenEditGame(Game game)
        {
            this.IsNewGame = false;
            this.Game = game;
            Navigator.NavigateTo<EditGamePage>();
        }

        private void OnSaveGame()
        {
            if (this.IsNewGame)
            {
                App.ViewModel.Games.Items.Add(this.Game);
            }
            
            FileManager.SaveGameFileAsync(this.Game);
            Navigator.NavigateBack();
        }
    }
}
