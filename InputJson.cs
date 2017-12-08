
using System.Collections.Generic;


namespace GA
{
    public class InputJson
    {
        public class Shift
        {
            public int id { get; set; }
            public int cost { get; set; }
            public int start { get; set; }
            public int duration { get; set; }
            public double modifier { get; set; }
            public int curProviders { get; set; }
            public int minProviders { get; set; }
            public int maxProviders { get; set; }
            public List<bool> days { get; set; }
        }

        public class Resource
        {
            public int id { get; set; }
            public string name { get; set; }
            public int minProviders { get; set; }
            public int maxProviders { get; set; }
            public List<Shift> shifts { get; set; }
        }

        public class Station
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool dummy { get; set; }
            public int execution { get; set; }
            public int overflow { get; set; }
            public bool requireBed { get; set; }
            public bool priorityQueue { get; set; }
            public List<int> timeBeforeLeave { get; set; }
            public int distType { get; set; }
            public List<object> resources { get; set; }
        }

        public class Edge
        {
            public int id { get; set; }
            public int from { get; set; }
            public int to { get; set; }
            public int type { get; set; }
            public int priority { get; set; }
            public List<int> p { get; set; }
        }

        public class RootObject
        {
            public int start { get; set; }
            public int end { get; set; }
            public int endResult { get; set; }
            public int beds { get; set; }
            public int cbeds { get; set; }
            public int seats { get; set; }
            public int currentTime { get; set; }
            public int currentDay { get; set; }
            public List<List<List<double>>> incomingRates { get; set; }
            public int patientsHours { get; set; }
            public List<object> patients { get; set; }
            public List<Resource> resources { get; set; }
            public List<Station> stations { get; set; }
            public List<Edge> edges { get; set; }
            public List<object> timededges { get; set; }
        }
    }
}
