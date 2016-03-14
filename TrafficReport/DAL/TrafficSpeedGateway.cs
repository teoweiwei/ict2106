using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class TrafficSpeedGateway : DataGateway<tblTrafficSpeed>
    {
        public List<tblTrafficSpeed> SaveSpeedData(List<LTADataMallModel.SpeedData> dataList)
        {
            List<tblTrafficSpeed> savedSpeedData = new List<tblTrafficSpeed>();
            IQueryable<tblRoadName> validRoadNameList = db.tblRoadNames.Where(l => l.rnLocation != null);

            for (int i = 0; i < dataList.Count(); i++)
            {
                int roadID = dataList[i].LinkID;
                Boolean isValid = validRoadNameList.Where(d => d.rnID == roadID).ToList().Count() > 0;

                if(isValid)
                {
                    tblTrafficSpeed speedData = new tblTrafficSpeed();

                    speedData.tsRoadName = roadID;
                    speedData.tsDateTime = dataList[i].CreateDate;
                    speedData.tsMinSpeed = dataList[i].MinimumSpeed;
                    speedData.tsMaxSpeed = dataList[i].MaximumSpeed;

                    Insert(speedData);
                    savedSpeedData.Add(speedData);
                }
            }

            return savedSpeedData;
        }
    }
}