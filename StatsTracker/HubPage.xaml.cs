using StatsTracker.Common;
using StatsTracker.Data;
using StatsTracker.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace StatsTracker
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : StatsTracker.Common.LayoutAwarePage
    {
        public HubPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
            //var dataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            //var dataGroups = new StatsDataSource().AllGroups;
            var dataGroups = App.ViewModel.AllGroups;
            this.DefaultViewModel["Groups"] = dataGroups;
            this.DefaultViewModel["HubViewModel"] = App.ViewModel;
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var game = e.ClickedItem as Game;
            if (game != null)
            {
                var playerStats = App.ViewModel.Players.Items.Cast<Player>().Select(p => new PlayerStat(p));
                var gameViewModel = new GameDetailViewModel(game,  new ObservableCollection<PlayerStat>(playerStats));
                App.ViewModel.SelectedGame = gameViewModel;
                this.Frame.Navigate(typeof(GameDetailPage));
            }
        }

        private void AddGameDialog_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            this.AddGameDialog.IsOpen = false;
        }
    }
}
