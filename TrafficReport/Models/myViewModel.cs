using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrafficReport.Models
{

    public class myViewModel
    {

        public tblRoadName tblRoadName { get; set; }
        public tblTrafficAccident tblTrafficAccident { get; set; }
        public tblLocationName tblLocationName { get; set; }
        public tblRainfall tblRainfall { set; get; }
        List<string> regionList { set; get; }
        
        //DateTime Date { get; set; }
    }
}