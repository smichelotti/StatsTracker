using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker.Data
{
    public class StatsDesignTimeDataSource : StatsDataSource
    {
        public StatsDesignTimeDataSource()
        {
            StatsDesignTimeDataService.PopulateWithData(this);
        }
    }

    public static class StatsDesignTimeDataService
    {
        public static void PopulateWithData(StatsDataSource dataSource)
        {
            var gamesGroup = new AppDataGroup("Games");
            var game1 = new Game("Baltimore Alliance", new DateTime(2012, 1, 1));
            game1.SharksScore = 59;
            game1.OpponentScore = 27;
            gamesGroup.Items.Add(game1);
            gamesGroup.Items.Add(new Game("Baltimore Stars", new DateTime(2013, 1, 2)));
            gamesGroup.Items.Add(new Game("Pikesville", new DateTime(2013, 1, 3)));
            gamesGroup.Items.Add(new Game("Ravens A", new DateTime(2013, 1, 4)));
            gamesGroup.Items.Add(new Game("6th Man Warriors", new DateTime(2013, 1, 5)));
            dataSource.AllGroups.Add(gamesGroup);

            var playersGroup = new AppDataGroup("Players");
            playersGroup.Items.Add(new Player("Tyler Clark", 0, "ms-appx:///Assets/0-tyler.jpg"));
            playersGroup.Items.Add(new Player("Mazae Blake", 1, "ms-appx:///Assets/1-mazae.jpg"));
            playersGroup.Items.Add(new Player("Justin Michelotti", 12, "ms-appx:///Assets/12-justinM.jpg"));
            dataSource.AllGroups.Add(playersGroup);
        }
    }

    public class GameDesignTimeViewModel : Game
    {
        public GameDesignTimeViewModel() : base ("Team Takeover", DateTime.Today)
        {
            //this.Score = "W 34-29";
            this.SharksScore = 34;
            this.OpponentScore = 29;
        }
    }
}
