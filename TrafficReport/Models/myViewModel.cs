﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrafficReport.Models
{

    public class NewViewModel
    {

        public tblRoadName tblRoadName { get; set; }
        public tblTrafficAccident tblTrafficAccident { get; set; }
        public tblLocationName tblLocationName { get; set; }
        public tblRainfall tblRainfall { set; get; }
        public tblTrafficSpeed tblTrafficSpeed { set; get; }
        
        //DateTime Date { get; set; }
    }
}