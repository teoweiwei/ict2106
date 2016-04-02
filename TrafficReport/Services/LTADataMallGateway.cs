using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TrafficReport.Models;

namespace TrafficReport.Services
{
    //Service gateway to access LTADataMall web service
    public class LTADataMallGateway : ILTADataMallGateway
    {
        private const string ACCIDENT_URL = "http://datamall.mytransport.sg/ltaodataservice.svc/IncidentSet";
        private const string SPEED_URL = "http://datamall.mytransport.sg/ltaodataservice.svc/TrafficSpeedBandSet";
        private const string ACCOUNT_KEY = "73yoKcEiJDZCbxL3KOxLJw==";
        private const string UNIQUE_USER_ID = "4eab90a3-02d5-4362-8dc5-0cc5f32325b0";

        //Request web service for traffic accident data
        public List<LTADataMallModel.AccidentData> GetLTAAccidentData()
        {
            //Convert JSON data into a data model list
            LTADataMallModel.LTADataMallAccidentData ltaDataMallAccidentData = JsonConvert.DeserializeObject<LTADataMallModel.LTADataMallAccidentData>(LTADataMallRequest(ACCIDENT_URL));

            //Return data model list
            return ltaDataMallAccidentData.d;
        }

        //Request web service for traffic speed and road name data
        public List<LTADataMallModel.SpeedData> GetLTASpeedData()
        {
            List<LTADataMallModel.SpeedData> speedData = new List<LTADataMallModel.SpeedData>();
            int skipCount = 0;

            //Convert JSON data into a data model list
            LTADataMallModel.LTADataMallSpeedBandData ltaDataMallSpeedBandData = JsonConvert.DeserializeObject<LTADataMallModel.LTADataMallSpeedBandData>(LTADataMallRequest(SPEED_URL + "?$skip=" + skipCount.ToString()));

            //Continuously request for data until no data is receive
            do
            {
                //Append new data into model
                for (int i = 0; i < ltaDataMallSpeedBandData.d.Count(); i++)
                {
                    speedData.Add(ltaDataMallSpeedBandData.d[i]);
                }

                skipCount += 50;
                ltaDataMallSpeedBandData = JsonConvert.DeserializeObject<LTADataMallModel.LTADataMallSpeedBandData>(LTADataMallRequest(SPEED_URL + "?$skip=" + skipCount.ToString()));

            } while (ltaDataMallSpeedBandData.d.Count != 0);

            //Return data model
            return speedData;
        }

        //Actual web service request procedure
        private string LTADataMallRequest(string url)
        {
            try
            {
                string jsonData;

                //Setting up HTTP request message
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("AccountKey", ACCOUNT_KEY);
                request.Headers.Add("UniqueUserID", UNIQUE_USER_ID);
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.Method = "GET";

                //Reponse from web service
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonData = reader.ReadToEnd();
                }

                //Return JSON data
                return jsonData;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}