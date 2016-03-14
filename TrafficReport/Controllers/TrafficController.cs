using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrafficReport.DAL;
using TrafficReport.Models;

namespace TrafficReport.Controllers
{
    public class TrafficController : Controller
    {
        private LocationNameGateway locationNameGateway = new LocationNameGateway();
        private TrafficReportContext db = new TrafficReportContext();

        // GET: Traffic
        public ActionResult Index()
        {
            
            List<string> regionList = new List<string>();
            regionList.Add("North");
            regionList.Add("South");
            regionList.Add("East");
            regionList.Add("West");

            ViewData["regions"] = new SelectList(regionList);

            List<string> roadName = new List<string>();
            roadName.Add("Jurong East Avenue 1");
            roadName.Add("Woodlands Drive 43");
            roadName.Add("Tampines Central 2");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);

            


            return View();
        }
        public ActionResult HandleForm(string regions, string roadNames, string period, string reportType)
        {
            ViewData["regions"] = regions;
            ViewData["roadNames"] = roadNames;
            ViewData["period"] = period;
            ViewData["reportType"] = reportType;
            //var model = (
            //    from db 
            //    )
            var accidentlist = (from rn in db.tblRoadNames
                                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                                where rn.rnRoadName == roadNames && rn.rnID == ta.taRoadName
                                
                                select new myViewModel
                                {
                                    //RoadName = rn.rnRoadName
                                    tblRoadName = rn,
                                    tblTrafficAccident = ta                                    
                                    
                                }
                                );

            //var test = new myViewModel();
            //test.RoadName = "lol";
            
            List<tblLocationName> data = db.tblLocationNames.ToList();
            ViewData["data"] = data;
            //return View("FormResults", locationNameGateway.SelectAll());
            return View("FormResults", accidentlist);
        }

        // GET: Traffic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = db.tblLocationNames.Find(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // GET: Traffic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Traffic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lnID,lnLocationName,lnRegion")] tblLocationName tblLocationName)
        {
            if (ModelState.IsValid)
            {
                db.tblLocationNames.Add(tblLocationName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblLocationName);
        }

        // GET: Traffic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = db.tblLocationNames.Find(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // POST: Traffic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lnID,lnLocationName,lnRegion")] tblLocationName tblLocationName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLocationName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblLocationName);
        }

        // GET: Traffic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = db.tblLocationNames.Find(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // POST: Traffic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLocationName tblLocationName = db.tblLocationNames.Find(id);
            db.tblLocationNames.Remove(tblLocationName);
            db.SaveChanges();
            return RedirectToAction("Index");
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
