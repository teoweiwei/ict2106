using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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

            List<SelectListItem> regionList = new List<SelectListItem>();
            // List<myViewModel> regionList = new List<myViewModel>();
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new  { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {   
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }

            
            ViewBag.regions = regionList;
            

            List<string> roadName = new List<string>();
            roadName.Add("Jurong East Avenue 1");
            roadName.Add("Woodlands Drive 43");
            roadName.Add("Tampines Central 2");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);
            DateTime todaysDate = DateTime.Now.Date;

            var initial = (
                from rn in db.tblRoadNames
                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                where DbFunctions.DiffDays(ta.taDateTime, todaysDate) == 0 && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0

                select new myViewModel
                {
                    tblRoadName = rn,
                    tblTrafficAccident = ta,
                    tblLocationName = ln,
                    tblRainfall = rf
                }
                );


            return View(initial);
        }
        [HttpPost]
        public ActionResult HandleForm(string regions, string roadNames, string period, string reportType)
        {
            List<SelectListItem> regionList = new List<SelectListItem>();
            // List<myViewModel> regionList = new List<myViewModel>();
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }


            //ViewData["regions"] = regionList;
            ViewBag.regions = regionList;


            List<string> roadName = new List<string>();
            roadName.Add("Jurong East Avenue 1");
            roadName.Add("Woodlands Drive 43");
            roadName.Add("Tampines Central 2");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);
            
            DateTime todayDate = DateTime.Now;
            int periodDuration = 0;
            if (period.Equals("1month"))
            {
                periodDuration = -1;
            }else if(period.Equals("3month"))
            {
                periodDuration = -3;
            }
            else if (period.Equals("6month"))
            {
                periodDuration = -6;
            }else if (period.Equals("1year"))
            {
                periodDuration = -12;
            }
            
            DateTime comparingDates = DateTime.Today.AddMonths(periodDuration);
           
            
            var accidentlist = (
                                from rn in db.tblRoadNames
                                join ta in db.tblTrafficAccidents on rn.rnID equals ta.taRoadName
                                join ln in db.tblLocationNames on rn.rnLocation equals ln.lnID
                                join rf in db.tblRainfalls on rn.rnLocation equals rf.rfLocation

                                where rn.rnRoadName == roadNames && rn.rnID == ta.taRoadName && ta.taDateTime > comparingDates && DbFunctions.DiffDays(ta.taDateTime, rf.rfDate) == 0
                                    


                                select new myViewModel
                                {
                                    
                                    tblRoadName = rn,
                                    tblTrafficAccident = ta,
                                    tblLocationName = ln,
                                    tblRainfall = rf
                                    

                                }
                                );
            
            return View("Index", accidentlist);
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
