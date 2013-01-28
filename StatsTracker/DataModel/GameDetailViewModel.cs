﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StatsTracker.Common;
using StatsTracker.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StatsTracker.DataModel
{
    public class GameDetailViewModel : ViewModelBase
    {
        public GameDetailViewModel(Game game, ObservableCollection<PlayerStat> playerStats)
        {
            this.Game = game;
            this.PlayerStats = playerStats;
            this.SelectPlayers = new RelayCommand(() => OnSelectPlayers());
            this.SaveSelectedPlayers = new RelayCommand(() => OnSaveSelectedPlayers());
            this.SaveGame = new RelayCommand(() => OnSaveGame());
            this.DeleteGame = new RelayCommand(() => OnDeleteGame());
            this.SortPlayersByName = new RelayCommand(() => OnSortPlayersByName());
            this.SortPlayersByNumber = new RelayCommand(() => OnSortPlayersByNumber());
        }

        public RelayCommand SelectPlayers { get; private set; }
        public RelayCommand SaveSelectedPlayers { get; private set; }
        public RelayCommand SaveGame { get; private set; }
        public RelayCommand DeleteGame { get; private set; }
        public RelayCommand SortPlayersByName { get; private set; }
        public RelayCommand SortPlayersByNumber { get; private set; }
        
        public bool IsSelectPlayersDialogOpen { get; set; }
        public bool IsProgressVisible { get; set; }
        public Game Game { get; private set; }

        public ObservableCollection<PlayerStat> PlayerStats { get; set; }

        public void SaveGameToFile()
        {
            FileManager.SaveGameFileAsync(this.Game);
        }

        private void OnSelectPlayers()
        {
            this.IsSelectPlayersDialogOpen = true;
        }

        internal void OnSelectIndividualPlayer(SelectionChangedEventArgs e)
        {
            var addedItems = e.AddedItems.Cast<PlayerStat>();
            var removedItems = e.RemovedItems.Cast<PlayerStat>();
            foreach (var item in addedItems)
            {
                this.Game.PlayerStats.Add(item);
            }

            foreach (var item in removedItems)
            {
                this.Game.PlayerStats.Remove(item);
            }
        }

        private void OnSaveSelectedPlayers()
        {
            FileManager.SaveGameFileAsync(this.Game);
            this.IsSelectPlayersDialogOpen = false;
        }

        private void OnSaveGame()
        {
            FileManager.SaveGameFileAsync(this.Game);
        }

        private async void OnDeleteGame()
        {
            MessageDialog dialog = new MessageDialog("Are you sure you want to delete this game?","Delete?");
            dialog.Commands.Add(new UICommand("Yes", cmd =>
            {
                App.ViewModel.Games.Items.Remove(this.Game);
                FileManager.DeleteGameFileAsync(this.Game);
                Navigator.NavigateTo<HubPage>();
            }));
            dialog.Commands.Add(new UICommand("No"));

            await dialog.ShowAsync();
        }

        private void OnSortPlayersByName()
        {
            this.Game.PlayerStats.Sort(x => x.Player.Name);
            FileManager.SaveGameFileAsync(this.Game);
        }

        private void OnSortPlayersByNumber()
        {
            this.Game.PlayerStats.Sort(x => x.Player.Number);
            FileManager.SaveGameFileAsync(this.Game);
        }
    }
}
