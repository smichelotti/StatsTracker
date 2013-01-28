using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker.Data
{
    public class PlayerStat : EntityBase
    {
        public PlayerStat(Player player)
            : base(null)
        {
            this.Player = player;
        }

        public Player Player { get; set; }
        public bool IsVisible { get; set; }
        public int Steals { get; set; }
        public int Assists { get; set; }
        public int Rebounds { get; set; }
        public int Blocks { get; set; }
        public int FreeThrows { get; set; }

        public override bool Equals(object obj)
        {
            var otherStat = obj as PlayerStat;
            if (otherStat == null)
            {
                return base.Equals(obj);
            }
            return otherStat.Player.Number == this.Player.Number;
        }
    }
}
