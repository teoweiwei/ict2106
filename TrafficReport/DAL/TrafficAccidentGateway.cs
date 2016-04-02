using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrafficReport.Models;
using TrafficReport.Services;

namespace TrafficReport.DAL
{
    //This gateway perform database operation on tblTrafficAccident table
    public class TrafficAccidentGateway : DataGateway<tblTrafficAccident>
    {
        private GoogleReverseGeocodingGateway googleReverseGeocodingGateway = new GoogleReverseGeocodingGateway();

        //Save traffic acident data into database
        public List<tblTrafficAccident> SaveAccidentData(List<LTADataMallModel.AccidentData> dataList)
        {
            //List of successfully saved records
            List<tblTrafficAccident> savedAccidentData = new List<tblTrafficAccident>();

            //Loop to save each record
            for (int i = 0; i < dataList.Count(); i++)
            {
                //Extract only data type is Accident
                if (dataList[i].Type.Equals("Accident"))
                {
                    //Check if the accident record exist in the database base on accident's description
                    string checkDescription = dataList[i].Message;
                    if (data.Where(m => m.taDescription.Equals(checkDescription)).ToList().Count() == 0)
                    {
                        //Create model can assign value to respective fields
                        tblTrafficAccident accidentData = new tblTrafficAccident();
                        accidentData.taID = dataList[i].IncidentID;
                        accidentData.taDescription = dataList[i].Message;
                        accidentData.taDateTime = dataList[i].CreateDate.AddHours(8);
                        accidentData.taLat = dataList[i].Latitude;
                        accidentData.taLong = dataList[i].Longitude;

                        //Get road name from Google Geocoding web service by provide latitude and longitude coordinates
                        string roadName = googleReverseGeocodingGateway.GetRoadNameGeolocation(accidentData.taLat, accidentData.taLong);
                        
                        if (roadName != null)
                        {
                            //Only save accident record if the road name exist in the database
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

            //Return the list of saved records
            return savedAccidentData;
        }

        internal IQueryable<QueryViewModel> FilterDatabase(string regions, string roadNames, string period, string reportType)
        {
            var queryResults = InitModel();
            int periodDuration = 0;
            if (period.Equals("1month"))
            {
                periodDuration = -1;
                DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);

               queryResults = (
                                from rn in db.tblRoadNames
                                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                where rn.rnRoadName == roadNames && rn.rnID == ta.taRoadName && ta.taDateTime > comparingDates && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0
                                group rf by rf.rfDate.Day into Date
                                select new QueryViewModel
                                {
                                    date = Date.Key,
                                    rainfall = (double)Date.Average(value => value.rfValue),
                                    number = Date.Count(),
                                    roadName = roadNames
                                    
                                }
                                );
                
                
            }
            
            else if (period.Equals("1year") || period.Equals("3month") || period.Equals("6month"))
            {
                if (period.Equals("1year")){
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
                                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                where rn.rnRoadName == roadNames && rn.rnID == ta.taRoadName && ta.taDateTime > comparingDates && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0
                                group rf by rf.rfDate.Month into Date
                                select new QueryViewModel
                                {
                                   
                                    date = Date.Key,
                                    rainfall = (double)Date.Average(value => value.rfValue),
                                    number = Date.Count(),
                                    roadName = roadNames
                                    

                                }
                                );
            }
            
            
            return queryResults;
            
        }

        internal IQueryable<QueryViewModel> InitModel()
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
                 
                }
                );
            return initial;
        }

    }
}