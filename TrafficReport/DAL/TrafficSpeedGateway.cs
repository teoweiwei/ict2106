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

        internal IQueryable<myViewModel> filterDatabase(string regions, string roadNames, string period, string reportType)
        {

            int periodDuration = 0;
            if (period.Equals("1month"))
            {
                periodDuration = -1;
            }
            else if (period.Equals("3month"))
            {
                periodDuration = -3;
            }
            else if (period.Equals("6month"))
            {
                periodDuration = -6;
            }
            else if (period.Equals("1year"))
            {
                periodDuration = -12;
            }


            DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);

            var viewModel = (
                                   from rn in db.tblRoadNames
                                   join ts in db.tblTrafficSpeeds on rn.rnID equals ts.tsRoadName
                                   join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                   join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                   where rn.rnRoadName == roadNames && rn.rnID == ts.tsRoadName && ts.tsDateTime > comparingDates && DbFunctions.DiffDays(ts.tsDateTime, rf.rfDate) == 0
                                   && ((ts.tsMinSpeed + ts.tsMaxSpeed) / 2) < (rn.rnSpeedLimit / 2)

                                   select new myViewModel
                                   {

                                       tblRoadName = rn,
                                       tblTrafficSpeed = ts,
                                       tblLocationName = ln,
                                       tblRainfall = rf

                                   }
                                   );
            return viewModel;
        }

        }
    }