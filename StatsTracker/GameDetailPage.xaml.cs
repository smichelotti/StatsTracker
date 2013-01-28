using StatsTracker.Data;
using StatsTracker.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace StatsTracker
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class GameDetailPage : StatsTracker.Common.LayoutAwarePage
    {
        public GameDetailPage()
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
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            var selectedGame = App.ViewModel.SelectedGame;
            this.DataContext = selectedGame;
            this.playerSelectionGrid.ItemsSource = selectedGame.PlayerStats;
            this.isLoading = true;
            foreach (var item in selectedGame.Game.PlayerStats)
            {
                this.playerSelectionGrid.SelectedItems.Add(item);
            }
            this.isLoading = false;
        }
        private bool isLoading;

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            //var selectedItem = this.flipView.SelectedItem;
            // TODO: Derive a serializable navigation parameter and assign it to pageState["SelectedItem"]
            //var gameViewModel = this.DataContext as GameDetailViewModel;
            //gameViewModel.SaveGame.Execute(null);
        }
        
        private void OnOpponentTapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void OnPlayerSelectionGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.isLoading)
            {
                return;
            }
            var gameViewModel = this.DataContext as GameDetailViewModel;
            gameViewModel.OnSelectIndividualPlayer(e);
        }
    }
}
