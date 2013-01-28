using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker.Data
{
    public class Player : EntityBase
    {
        public Player(string name, int? number, string imagePath)
            : base(imagePath)
        {
            this.Name = name;
            this.Number = number;
        }

        public string Name { get; set; }
        public int? Number { get; set; }
    }
}
