using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrafficReport.Models;
using TrafficReport.Services;

namespace TrafficReport.DAL
{
    public class TrafficAccidentGateway : DataGateway<tblTrafficAccident>
    {
        private GoogleReverseGeocodingGateway googleReverseGeocodingGateway = new GoogleReverseGeocodingGateway();

        public List<tblTrafficAccident> SaveAccidentData(List<LTADataMallModel.AccidentData> data)
        {
            List<tblTrafficAccident> savedAccidentData = new List<tblTrafficAccident>();

            for (int i=0; i<data.Count(); i++)
            {
                if(data[i].Type.Equals("Accident"))
                {
                    string checkDescription = data[i].Message;
                    if (db.tblTrafficAccidents.Where(m => m.taDescription.Equals(checkDescription)).ToList().Count() == 0)
                    {
                        tblTrafficAccident accidentData = new tblTrafficAccident();
                        accidentData.taID = data[i].IncidentID;
                        accidentData.taDescription = data[i].Message;
                        accidentData.taDateTime = data[i].CreateDate;
                        accidentData.taLat = data[i].Latitude;
                        accidentData.taLong = data[i].Longitude;

                        string roadName = googleReverseGeocodingGateway.getGeolocationRoadName(accidentData.taLat, accidentData.taLong);

                        if(roadName != null)
                        {
                            int rnID = db.tblRoadNames.Where(r => r.rnRoadName.ToLower().Equals(roadName.ToLower())).ToList()[0].rnID;

                            accidentData.taRoadName = rnID;
                        }

                        Insert(accidentData);
                        savedAccidentData.Add(accidentData);
                    }
                }
            }

            return savedAccidentData;
        }
    }
}