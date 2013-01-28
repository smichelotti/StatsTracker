using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker.DataModel
{
    public partial class Roster
    {
        public Address address { get; set; }
        public DateTime? birthdate { get; set; }
        public List<object> contacts { get; set; }
        //public List<CustomData> custom_data { get; set; }
        public string first { get; set; }
        public string full_picture_url { get; set; }
        public string gender { get; set; }
        public int id { get; set; }
        public bool is_manager { get; set; }
        public bool is_owner { get; set; }
        public string last { get; set; }
        public bool non_player { get; set; }
        public int? number { get; set; }
        public string position { get; set; }
        //public List<RosterEmailAddress> roster_email_addresses { get; set; }
        //public List<RosterTelephoneNumber> roster_telephone_numbers { get; set; }
        public string thumbnail_picture_url { get; set; }
    }

    public class RosterRoot
    {
        public Roster roster { get; set; }
    }

    public class Address
    {
        public string address { get; set; }
        public object address2 { get; set; }
        public string city { get; set; }
        public object country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
}
