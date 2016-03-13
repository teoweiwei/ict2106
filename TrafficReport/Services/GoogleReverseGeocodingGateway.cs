using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using TrafficReport.Models;

namespace TrafficReport.Services
{
    public class GoogleReverseGeocodingGateway
    {
        private const string URL = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";
        private const string API_KEY = "AIzaSyC7f9VGd6gfZaJO3IAHZl05sHhFTl2V5UM";

        public string getGeolocationRoadName(double? lat, double? lng)
        {
            try
            {
                string jsonData;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL+lat.ToString() + "," + lng.ToString() + "&result_type=route&key=" + API_KEY);
                request.Accept = "application/json";
                request.Host = "maps.googleapis.com";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonData = reader.ReadToEnd();
                }

                GoogleReverseGeocodingModel.GeocodeingData geocodingData = JsonConvert.DeserializeObject<GoogleReverseGeocodingModel.GeocodeingData>(jsonData);

                if(geocodingData.status.Equals("OK"))
                {
                    return geocodingData.results[0].address_components[0].long_name;
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}