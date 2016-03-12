using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.Services
{
    public class LTADataMallGateway
    {
        private const string SPEED_URL = "http://datamall.mytransport.sg/ltaodataservice.svc/TrafficSpeedBandSet";
        private const string ACCIDENT_URL = "http://datamall.mytransport.sg/ltaodataservice.svc/IncidentSet";
        private const string ACCOUNT_KEY = "73yoKcEiJDZCbxL3KOxLJw==";
        private const string UNIQUE_USER_ID = "4eab90a3-02d5-4362-8dc5-0cc5f32325b0";


        public LTADataMallModel.LTADataMallSpeedBandData GetLTASpeedData()
        {

            /*LTADataMallModel.Metadata meta = new LTADataMallModel.Metadata();
            meta.type = "A";
            meta.uri = "B";

            LTADataMallModel.SpeedData speedData = new LTADataMallModel.SpeedData();
            speedData.__MetaData = meta;
            speedData.Band = 1;

            LTADataMallModel.LTADataMallSpeedBandData data = new LTADataMallModel.LTADataMallSpeedBandData();
            data.ltaDataMallSpeedBandDataList = new List<LTADataMallModel.SpeedData>();
            data.ltaDataMallSpeedBandDataList.Add(speedData);
            data.ltaDataMallSpeedBandDataList.Add(speedData);
            data.ltaDataMallSpeedBandDataList.Add(speedData);

            string itemsSerialized2 = JsonConvert.SerializeObject(data);*/





            string itemsSerialized = LTADataMallRequest(SPEED_URL);

            LTADataMallModel.LTADataMallSpeedBandData ltaDataMallSpeedBandData = JsonConvert.DeserializeObject<LTADataMallModel.LTADataMallSpeedBandData>(itemsSerialized);

            object dict = JsonConvert.DeserializeObject<object>(itemsSerialized);
            


            //LTADataMallModel.LTADataMallSpeedBandData ltaDataMallSpeedBandData = JsonConvert.DeserializeObject<LTADataMallModel.LTADataMallSpeedBandData>(itemsSerialized);

            return ltaDataMallSpeedBandData;
        }

        public void GetLTAAccidentData()
        {
            
        }

        private string LTADataMallRequest(string url)
        {
            try
            {
                string jsonData;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("AccountKey", ACCOUNT_KEY);
                request.Headers.Add("UniqueUserID", UNIQUE_USER_ID);
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonData = reader.ReadToEnd();
                }

                return jsonData;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}