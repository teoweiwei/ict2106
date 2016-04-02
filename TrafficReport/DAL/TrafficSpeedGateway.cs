using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    //This gateway perform database operation on tblTrafficSpeed table
    public class TrafficSpeedGateway : DataGateway<tblTrafficSpeed>
    {
        //Save traffic speed data into database
        public List<tblTrafficSpeed> SaveSpeedData(List<LTADataMallModel.SpeedData> dataList)
        {
            //List of successfully saved records
            List<tblTrafficSpeed> savedSpeedData = new List<tblTrafficSpeed>();

            //List of road name traffic speed data to be saved. Road name with location ID assigned will be saved for this scope of project, else all Singapore's road speed date will be saved
            IQueryable<tblRoadName> validRoadNameList = db.tblRoadNames.Where(l => l.rnLocation != null);

            //Loop to save each record
            for (int i = 0; i < dataList.Count(); i++)
            {
                //Check if current record in a vaild road to be saved
                int roadID = dataList[i].LinkID;
                Boolean isValid = validRoadNameList.Where(d => d.rnID == roadID).ToList().Count() > 0;

                //Save traffic speed record which is inside the valid list
                if(isValid)
                {
                    //Create model can assign value to respective fields
                    tblTrafficSpeed speedData = new tblTrafficSpeed();
                    speedData.tsRoadName = roadID;
                    speedData.tsDateTime = dataList[i].CreateDate;
                    speedData.tsMinSpeed = dataList[i].MinimumSpeed;
                    speedData.tsMaxSpeed = dataList[i].MaximumSpeed;

                    Insert(speedData);
                    savedSpeedData.Add(speedData);
                }
            }

            //Return the list of saved records
            return savedSpeedData;
        }

        internal IQueryable<QueryViewModel> filterDatabase(string regions, string roadNames, string period, string reportType)
        {

            //int periodDuration = 0;
            //if (period.Equals("1month"))
            //{
            //    periodDuration = -1;
            //}
            //else if (period.Equals("3month"))
            //{
            //    periodDuration = -3;
            //}
            //else if (period.Equals("6month"))
            //{
            //    periodDuration = -6;
            //}
            //else if (period.Equals("1year"))
            //{
            //    periodDuration = -12;
            //}


            //DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);

            //var viewModel = (
            //                       from rn in db.tblRoadNames
            //                       join ts in db.tblTrafficSpeeds on rn.rnID equals ts.tsRoadName
            //                       join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
            //                       join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

            //                       where rn.rnRoadName == roadNames && rn.rnID == ts.tsRoadName && ts.tsDateTime > comparingDates && DbFunctions.DiffDays(ts.tsDateTime, rf.rfDate) == 0
            //                       && ((ts.tsMinSpeed + ts.tsMaxSpeed) / 2) < (rn.rnSpeedLimit / 2)

            //                       select new QueryViewModel
            //                       {

            //                           //tblRoadName = rn,
            //                           //tblTrafficSpeed = ts,
            //                           //tblLocationName = ln,
            //                           //tblRainfall = rf

            //                       }
            //                       );
            //return viewModel;

            var queryResults = initModel();
            int periodDuration = 0;
            if (period.Equals("1month"))
            {
                periodDuration = -1;
                DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);

                queryResults = (
                                 from rn in db.tblRoadNames
                                 join ts in db.tblTrafficSpeeds on rn.rnID equals ts.tsRoadName
                                 join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                 join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                 where rn.rnRoadName == roadNames && rn.rnID == ts.tsRoadName && ts.tsDateTime > comparingDates && DbFunctions.DiffDays(ts.tsDateTime, rf.rfDate) == 0
                                                        && ((ts.tsMinSpeed + ts.tsMaxSpeed) / 2) < (rn.rnSpeedLimit / 2)
                                 //group rf by rf.rfDate.Day into Date
                                 group new {rn, ts, ln, rf} by new { rn.rnRoadName, ts.tsMaxSpeed, ts.tsMinSpeed, ts.tsDateTime, rn.rnSpeedLimit, rf.rfValue} into grp
                                 select new QueryViewModel
                                 {

                                    //tblRoadName = rn,
                                    //tblTrafficAccident = ta,
                                    //tblLocationName = ln,
                                    //tblRainfall = rf
                                     
                                     dateTime = grp.Key.tsDateTime,
                                     rainfall = (double)grp.Key.rfValue,
                                     number = (int)((grp.Key.tsMinSpeed + grp.Key.tsMaxSpeed) / 2),
                                     roadName = grp.Key.rnRoadName
                                 }
                                 );


                //new QueryViewModel
                //{
                //    date = item.date,
                //    rainfall = item.rainfall,
                //    number = item.number
                //};

            }
            //else if (period.Equals("3month"))
            //{
            //    periodDuration = -3;
            //}
            //else if (period.Equals("6month"))
            //{
            //    periodDuration = -6;
            //}
            else if (period.Equals("1year") || period.Equals("3month") || period.Equals("6month"))
            {
                if (period.Equals("1year"))
                {
                    periodDuration = -12;
                }
                if (period.Equals("3month"))
                {
                    periodDuration = -3;
                }
                if (period.Equals("6month"))
                {
                    periodDuration = -6;
                }

                DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);

                queryResults = (
                                 from rn in db.tblRoadNames
                                 join ts in db.tblTrafficSpeeds on rn.rnID equals ts.tsRoadName
                                 join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                 join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                 where rn.rnRoadName == roadNames && rn.rnID == ts.tsRoadName && ts.tsDateTime > comparingDates && DbFunctions.DiffDays(ts.tsDateTime, rf.rfDate) == 0
                                                        && ((ts.tsMinSpeed + ts.tsMaxSpeed) / 2) < (rn.rnSpeedLimit / 2)
                                 //group rf by rf.rfDate.Day into Date
                                 group new { rn, ts, ln, rf } by new { rn.rnRoadName, ts.tsMaxSpeed, ts.tsMinSpeed, rf.rfDate.Month, rn.rnSpeedLimit, rf.rfValue, ((ts.tsMinSpeed + ts.tsMaxSpeed) / 2).Value } into grp
                                 group grp by grp.Key.Month into col
                                 select new QueryViewModel
                                 {

                                     //tblRoadName = rn,
                                     //tblTrafficAccident = ta,
                                     //tblLocationName = ln,
                                     //tblRainfall = rf

                                     //date = grp.Key.Month,
                                     //rainfall = (double)grp.Average(value => value.rf.rfValue),
                                     ////rainfall = (double)grp.Key.rfValue,
                                     ////number = (int)grp.Average(avg => avg.ts.),
                                     //roadName = grp.Key.rnRoadName

                                     date = col.Key,
                                     rainfall = (double)col.Average(value => value.Key.rfValue),
                                     //rainfall = (double)grp.Key.rfValue,
                                     //number = (int)grp.Average(avg => avg.ts.),
                                     number = (int)col.Average(avg => avg.Key.Value),
                                     roadName = roadNames

                                 }
                                 );
            }

            return queryResults;
        }

        internal IQueryable<QueryViewModel> initModel()
        {
            DateTime todaysDate = DateTime.Now.Date;

            var initial = (
                from rn in db.tblRoadNames
                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation


                where DbFunctions.DiffDays(ta.taDateTime, todaysDate) == 0 && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0

                select new QueryViewModel
                {
                    //tblRoadName = rn,
                    //tblTrafficAccident = ta,
                    //tblLocationName = ln,
                    //tblRainfall = rf
                }
                );
            return initial;
        }

    }
    }