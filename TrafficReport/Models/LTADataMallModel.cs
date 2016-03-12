using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrafficReport.Models
{
    public class LTADataMallModel
    {
        public class Metadata
        {
            public string uri { get; set; }
            public string type { get; set; }
        }

        public class SpeedData
        {
            public Metadata __MetaData { get; set; }
            public string TrafficSpeedBandID { get; set; }
            public int LinkID { get; set; }
            public string RoadName { get; set; }
            public string RoadCategory { get; set; }
            public int Band { get; set; }
            public int MinimumSpeed { get; set; }
            public int MaximumSpeed { get; set; }
            public string Location { get; set; }
            public string Summary { get; set; }
            public DateTime CreateDate { get; set; }
        }

        public class LTADataMallSpeedBandData
        {
            public List<SpeedData> ltaDataMallSpeedBandDataList { get; set; }
        }
    }
}