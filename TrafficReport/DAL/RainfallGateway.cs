﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using TrafficReport.Models;
using System.Data.Entity;

namespace TrafficReport.DAL
{
    //This gateway perform database operation on tblRainfall table
    public class RainfallGateway : DataGateway<tblRainfall>
    {
        //Save rainfall data into database from CSV file
        public IEnumerable<tblRainfall> SaveRainfallData(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                if (upload.FileName.EndsWith(".csv"))
                {
                    List<tblRainfall> savedRainfallData = new List<tblRainfall>();

                    Stream stream = upload.InputStream;
                    StreamReader read = new StreamReader(stream);

                    List<string> row = new List<string>();
                    read.ReadLine();
                    
                    //Read each row of data in the CSV file till end
                    while (!read.EndOfStream)
                    {
                        tblRainfall rainfallData = new tblRainfall();
                        row = read.ReadLine().Split(',').ToList();

                        //Get location id from tblLocationNames based on location name
                        string locationName = row[0];
                        int locationId = db.tblLocationNames.Where(l => l.lnLocationName.Equals(locationName)).ToList()[0].lnID;

                        //Parse date
                        rainfallData.rfDate = DateTime.ParseExact(row[3] + "/" + row[2] + "/" + row[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                        rainfallData.rfLocation = locationId;

                        //Insert value as 0 if no rainfall value else get actual value
                        if(row[4].Equals("-"))
                        {
                            rainfallData.rfValue = 0;
                        }
                        else
                        {
                            rainfallData.rfValue = decimal.Parse(row[4]);
                        }

                        //Add record into DB
                        Insert(rainfallData);
                        savedRainfallData.Add(rainfallData);
                    }

                    //Return the list of saved records
                    return savedRainfallData;
                }
            }
            return null;
        }

        internal IQueryable<QueryViewModel> InitModel()
        {
            DateTime todaysDate = DateTime.Now.Date;

            var initial = (
                                   from rn in db.tblRoadNames
                                   join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                   join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                   where DbFunctions.DiffDays(rf.rfDate, todaysDate) == 0

                                   select new QueryViewModel
                                   {
                                       
                                   }
                );
            return initial;
        }

        internal IQueryable<QueryViewModel> FilterDatabase(string regions, string roadNames, string period)
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

            {
                var queryResults = (
                                   from rn in db.tblRoadNames
                                   join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                   join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                   where rn.rnRoadName == roadNames && rf.rfDate > comparingDates
                                   group rf by rf.rfDate.Month into Date
                                   select new QueryViewModel
                                   {
                                       date = Date.Key,
                                       rainfall = (double)Date.Average(value => value.rfValue),
                                       roadName = roadNames


                                   }
                                   );
                return queryResults;
            }
        }
    }
}