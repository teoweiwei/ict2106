using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TrafficReport.Models;
using TrafficReport.Services;

namespace TrafficReport.DAL
{
    public class TrafficAccidentGateway : DataGateway<tblTrafficAccident>
    {
        private GoogleReverseGeocodingGateway googleReverseGeocodingGateway = new GoogleReverseGeocodingGateway();

        public List<tblTrafficAccident> SaveAccidentData(List<LTADataMallModel.AccidentData> dataList)
        {
            List<tblTrafficAccident> savedAccidentData = new List<tblTrafficAccident>();

            for (int i = 0; i < dataList.Count(); i++)
            {
                if (dataList[i].Type.Equals("Accident"))
                {
                    string checkDescription = dataList[i].Message;
                    if (data.Where(m => m.taDescription.Equals(checkDescription)).ToList().Count() == 0)
                    {
                        tblTrafficAccident accidentData = new tblTrafficAccident();
                        accidentData.taID = dataList[i].IncidentID;
                        accidentData.taDescription = dataList[i].Message;
                        accidentData.taDateTime = dataList[i].CreateDate.AddHours(8);
                        accidentData.taLat = dataList[i].Latitude;
                        accidentData.taLong = dataList[i].Longitude;

                        string roadName = googleReverseGeocodingGateway.GetRoadNameGeolocation(accidentData.taLat, accidentData.taLong);

                        if (roadName != null)
                        {
                            Boolean idExist = db.tblRoadNames.Where(r => r.rnRoadName.ToLower().Equals(roadName.ToLower())).ToList().Count() != 0;

                            if (idExist)
                            {
                                accidentData.taRoadName = db.tblRoadNames.Where(r => r.rnRoadName.ToLower().Equals(roadName.ToLower())).ToList()[0].rnID;
                            }
                        }

                        Insert(accidentData);
                        savedAccidentData.Add(accidentData);
                    }
                }
            }

            return savedAccidentData;
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

            var accidentlist = (
                               from rn in db.tblRoadNames
                               join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                               join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                               join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                               where rn.rnRoadName == roadNames && rn.rnID == ta.taRoadName && ta.taDateTime > comparingDates && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0
                               
                               select new myViewModel
                               {

                                   tblRoadName = rn,
                                   tblTrafficAccident = ta,
                                   tblLocationName = ln,
                                   tblRainfall = rf


                               }
                               );
            return accidentlist;
        }

        internal IQueryable<myViewModel> initModel()
        {
            DateTime todaysDate = DateTime.Now.Date;

            var initial = (
                from rn in db.tblRoadNames
                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                where DbFunctions.DiffDays(ta.taDateTime, todaysDate) == 0 && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0

                select new myViewModel
                {
                    tblRoadName = rn,
                    tblTrafficAccident = ta,
                    tblLocationName = ln,
                    tblRainfall = rf
                }
                );
            return initial;
        }
        
    }
}