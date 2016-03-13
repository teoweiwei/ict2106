using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class RoadNameGateway : DataGateway<tblRoadName>
    {
        public List<tblRoadName> SaveRoadNameData(List<LTADataMallModel.SpeedData> dataList)
        {
            List<tblRoadName> savedRoadNameData = new List<tblRoadName>();

            for (int i = 0; i < dataList.Count(); i++)
            {
                tblRoadName roadName = new tblRoadName();

                roadName.rnID = dataList[i].LinkID;
                roadName.rnRoadName = dataList[i].RoadName;
                roadName.rnSpeedLimit = 50;

                Boolean checkIDNotExist = SelectById(dataList[i].LinkID) == null;
                Boolean checkRoadNameNotExist = data.Where(m => m.rnRoadName.ToLower().Equals(roadName.rnRoadName.ToLower())).ToList().Count() == 0;

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