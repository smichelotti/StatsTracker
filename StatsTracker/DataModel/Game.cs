using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker.Data
{
    public class Game : EntityBase
    {
        public Game() : this(null, DateTime.Today)
        {
        }

        public Game(string opponent, DateTime date) : this(opponent, date, null)
        {
        }

        public Game(string opponent, DateTime date, IEnumerable<PlayerStat> playerStats) : base("Assets/basketball.png")
        {
            this.Opponent = opponent;
            this.Date = date;
            if (playerStats == null)
            {
                this.PlayerStats = new ObservableCollection<PlayerStat>();
            }
            else
            {
                this.PlayerStats = new ObservableCollection<PlayerStat>(playerStats);
            }
        }

        public string Opponent { get; set; }
        public DateTime Date { get; set; }
        public string Score { get; set; }

        public ObservableCollection<PlayerStat> PlayerStats { get; set; }
    }
}
