using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class RoadNameGateway : DataGateway<tblRoadName>
    {
        public List<tblRoadName> SaveRoadNameData(List<LTADataMallModel.SpeedData> data)
        {
            List<tblRoadName> savedRoadNameData = new List<tblRoadName>();

            for (int i = 0; i < data.Count(); i++)
            {
                tblRoadName roadName = new tblRoadName();

                roadName.rnID = data[i].LinkID;
                roadName.rnRoadName = data[i].RoadName;
                roadName.rnSpeedLimit = 50;

                Boolean checkIDNotExist = SelectById(data[i].LinkID) == null;
                Boolean checkRoadNameNotExist = db.tblRoadNames.Where(m => m.rnRoadName.ToLower().Equals(roadName.rnRoadName.ToLower())).ToList().Count() == 0;

                if (checkIDNotExist && checkRoadNameNotExist)
                {
                    Insert(roadName);
                    savedRoadNameData.Add(roadName);
                }
            }

            return savedRoadNameData;
        }
    }
}