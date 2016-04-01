using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrafficReport.Models
{
    public class QueryViewModel
    {
        public int date { get; set; }
        public string roadName { get; set; }
        public int number { get; set; }
        public double rainfall { get; set; }
        public string monthName { get; set; }
        public DateTime dateTime { get; set; }


    }
}