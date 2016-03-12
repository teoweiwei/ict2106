using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.DAL
{
    public class TrafficAccidentGateway : DataGateway<tblTrafficAccident>
    {
        public List<tblTrafficAccident> SaveAccidentData(List<LTADataMallModel.AccidentData> data)
        {
            List<tblTrafficAccident> storedAccidentData = new List<tblTrafficAccident>();

            for (int i=0; i<data.Count(); i++)
            {
                if(data[i].Type.Equals("Accident"))
                {
                    int id = data[i].IncidentID;
                    if (db.tblTrafficAccidents.Where(m => m.taID == id).ToList().Count() == 0)
                    {
                        tblTrafficAccident accidentData = new tblTrafficAccident();
                        accidentData.taID = data[i].IncidentID;
                        accidentData.taDateTime = data[i].CreateDate;
                        accidentData.taDescription = data[i].Message;

                        Insert(accidentData);
                        storedAccidentData.Add(accidentData);
                    }
                }
            }

            return storedAccidentData;
        }
    }
}