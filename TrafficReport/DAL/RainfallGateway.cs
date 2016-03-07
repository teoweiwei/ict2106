using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class RainfallGateway : DataGateway<tblRainfall>
    {
        public IEnumerable<tblRainfall> Upload(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                if (upload.FileName.EndsWith(".csv"))
                {
                    List<tblRainfall> saveRecord = new List<tblRainfall>();

                    Stream stream = upload.InputStream;
                    StreamReader read = new StreamReader(stream);

                    List<string> row = new List<string>();
                    tblRainfall rainfallData = new tblRainfall();
                    read.ReadLine();
                    
                    while (!read.EndOfStream)
                    {
                        row = read.ReadLine().Split(',').ToList();

                        string locationName = row[0];
                        int locationId = db.tblLocationNames.Where(l => l.lnLocationName.Equals(locationName)).ToList()[0].lnID;

                        rainfallData.rfDate = DateTime.ParseExact(row[3] + "/" + row[2] + "/" + row[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                        rainfallData.rfLocation = locationId;
                        rainfallData.rfValue = decimal.Parse(row[4]);

                        //Add record into DB
                        Insert(rainfallData);
                        saveRecord.Add(rainfallData);
                    }

                    return saveRecord;
                }
            }
            return null;
        }
    }
}