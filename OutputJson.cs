
using System.Collections.Generic;

namespace GA
{
    class OutputJson
    {
        public class Resource
        {
            public int cost { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public List<double> util_hour { get; set; }
        }

        public class Station
        {
            public double LOS_avg { get; set; }
            public double? LOS_dev { get; set; }
            public List<double> LOS_esi { get; set; }
            public double LWBS_avg { get; set; }
            public double LWBS_dev { get; set; }
            public List<double> LWBS_esi { get; set; }
            public double NOP_avg { get; set; }
            public double NOP_dev { get; set; }
            public List<double> NOP_esi { get; set; }
            public bool dummy { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public List<double> queue_hour { get; set; }
            public double wait_avg { get; set; }
            public double wait_dev { get; set; }
            public List<double> wait_hour { get; set; }
        }

        public class RootObject
        {
            public double LBTC_avg { get; set; }
            public double LBTC_dev { get; set; }
            public List<double> LBTC_esi { get; set; }
            public double LOS_avg { get; set; }
            public double LOS_dev { get; set; }
            public List<double> LOS_esi { get; set; }
            public double NOP_avg { get; set; }
            public double NOP_dev { get; set; }
            public List<double> NOP_esi { get; set; }
            public List<double> beds_hour { get; set; }
            public List<double> cbeds_hour { get; set; }
            public int cost { get; set; }
            public int effort { get; set; }
            public int elapsed { get; set; }
            public string log { get; set; }
            public List<Resource> resources { get; set; }
            public List<int> seats_hour { get; set; }
            public List<Station> stations { get; set; }
            public double wait_avg { get; set; }
            public double wait_dev { get; set; }
            public List<double> wait_esi { get; set; }
        }
    }
}
