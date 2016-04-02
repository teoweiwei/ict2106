using System;
using System.Collections.Generic;
using System.Linq;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    //This gateway perform database operation on tblRoadName table
    public class RoadNameGateway : DataGateway<tblRoadName>
    {
        //Save road name data into database
        public List<tblRoadName> SaveRoadNameData(List<LTADataMallModel.SpeedData> dataList)
        {
            //List of successfully saved records
            List<tblRoadName> savedRoadNameData = new List<tblRoadName>();

            //Loop to save each record
            for (int i = 0; i < dataList.Count(); i++)
            {
                //Create model can assign value to respective fields
                tblRoadName roadName = new tblRoadName();
                roadName.rnID = dataList[i].LinkID;
                roadName.rnRoadName = dataList[i].RoadName;
                roadName.rnSpeedLimit = 50;

                //Check whether road name and ID exist in the database
                Boolean checkIDNotExist = SelectById(dataList[i].LinkID) == null;
                Boolean checkRoadNameNotExist = data.Where(m => m.rnRoadName.ToLower().Equals(roadName.rnRoadName.ToLower())).ToList().Count() == 0;

                //Save road name if no record exist in database
                if (checkIDNotExist && checkRoadNameNotExist)
                {
                    Insert(roadName);
                    savedRoadNameData.Add(roadName);
                }
            }

            //Return the list of saved records
            return savedRoadNameData;
        }
    }
}