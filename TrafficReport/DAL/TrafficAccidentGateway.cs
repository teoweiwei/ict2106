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

            for (int i=0; i< dataList.Count(); i++)
            {
                if(dataList[i].Type.Equals("Accident"))
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

                        string roadName = googleReverseGeocodingGateway.getGeolocationRoadName(accidentData.taLat, accidentData.taLong);

                        if(roadName != null)
                        {
                            Boolean idExist = db.tblRoadNames.Where(r => r.rnRoadName.ToLower().Equals(roadName.ToLower())).ToList().Count() != 0;

                            if(idExist)
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
    }
}