using System;
using System.Net;
using System.Web.Mvc;
using group12web.DAL;
using group12web.Models;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Web;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;

namespace group12web.Controllers
{
    public class TrafficAccidentsController : Controller
    {
        private group12Context db = new group12Context();
        private TrafficAccidentsGateway TrafficAccidentsGate = new TrafficAccidentsGateway();

        //public string ACCOUNT_KEY { get; private set; }
        //public string UNIQUE_USER_ID { get; private set; }

        // GET: TrafficAccidents
        public ActionResult Index()
        {
            getTrafficData();
            return View();
            // return View(TrafficAccidentsGate.SelectAll());
        }

        // GET: TrafficAccidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrafficAccident trafficAccident = TrafficAccidentsGate.SelectById(id);
            if (trafficAccident == null)
            {
                return HttpNotFound();
            }
            return View(trafficAccident);
        }

        // GET: TrafficAccidents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrafficAccidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoadName,Date,Long,Lat,Description")] TrafficAccident trafficAccident)
        {
            if (ModelState.IsValid)
            {
                TrafficAccidentsGate.Insert(trafficAccident);
                return RedirectToAction("Index");
            }

            return View(trafficAccident);
        }

        // GET: TrafficAccidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrafficAccident trafficAccident = TrafficAccidentsGate.SelectById(id);
            if (trafficAccident == null)
            {
                return HttpNotFound();
            }
            return View(trafficAccident);
        }

        // POST: TrafficAccidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoadName,Date,Long,Lat,Description")] TrafficAccident trafficAccident)
        {
            if (ModelState.IsValid)
            {
                TrafficAccidentsGate.Update(trafficAccident);
                return RedirectToAction("Index");
            }
            return View(trafficAccident);
        }

        // GET: TrafficAccidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrafficAccident trafficAccident = TrafficAccidentsGate.SelectById(id);
            if (trafficAccident == null)
            {
                return HttpNotFound();
            }
            return View(trafficAccident);
        }

        // POST: TrafficAccidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrafficAccidentsGate.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: TrafficAccidents/Delete/5
        [HttpPost, ActionName("getTrafficData")]
        public void getTrafficData()
        {
            string postData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://datamall.mytransport.sg/ltaodataservice.svc/IncidentSet");
            request.Headers.Add("AccountKey", "73yoKcEiJDZCbxL3KOxLJw==");
            request.Headers.Add("UniqueUserID", "4eab90a3-02d5-4362-8dc5-0cc5f32325b0");
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Method = "GET";
  
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                System.Diagnostics.Debug.WriteLine("Response stream received. : LTA INCIDENT SET DATA " + DateTime.Now.ToString("hh: mm:ss tt"));

                //JsonObject test = JsonObject.Parse(readStream);
                String jsonResponse = readStream.ReadToEnd();
                //Console.WriteLine("First: " + test["__metadata"]
                String[] responseSplit = jsonResponse.Split('}');
                int loopCounter = 0;
                foreach (string i in responseSplit)
                {
                    if (loopCounter % 2 == 1)
                    {
                        char[] delimiter = { ',', ' ' };
                        string jsonRow = i.TrimStart(delimiter);
                        IncidentSet incidentSet = JsonConvert.DeserializeObject<incidentSet>("{" + jsonRow + "}");
                        TrafficAccidentsGate.Insert(trafficAccident);
                    }
                    loopCounter++;
                }
                response.Close();
                readStream.Close();
            }
            catch (SocketException e)
            {
                String exp = e.Message;
                // Throw the WebException with no parameters.
                throw new WebException();
            }

        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
