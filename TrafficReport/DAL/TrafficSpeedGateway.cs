using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class TrafficSpeedGateway : DataGateway<tblTrafficSpeed>
    {
        public List<tblTrafficSpeed> SaveSpeedData(List<LTADataMallModel.SpeedData> data)
        {
            List<tblTrafficSpeed> savedSpeedData = new List<tblTrafficSpeed>();

            for (int i = 0; i < data.Count(); i++)
            {
                
            }

            return savedSpeedData;
        }
    }
}