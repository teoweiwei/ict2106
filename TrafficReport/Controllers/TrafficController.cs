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
        private TrafficAccidentGateway trafficAccidentGateway = new TrafficAccidentGateway();
        private TrafficSpeedGateway trafficSpeedGateway = new TrafficSpeedGateway();
        
        private TrafficReportContext db = new TrafficReportContext();
        public TrafficController()
        {
        }
        // GET: Traffic

        public ActionResult Index(String region)
        {   
            List<SelectListItem> LocationUpdate = new List<SelectListItem>();
            List<SelectListItem> regionList = new List<SelectListItem>();

            //inserts the distinct regions from the database
            regionList.Add(new SelectListItem { Value = "Choose", Text = "<--  Region  -->" });
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new  { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {   
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }

            
            ViewBag.regions = regionList;
                        
            // insert a placeholder for the roadnames (to populate after the ser seleted a region)
            LocationUpdate.Add(new SelectListItem { Value = "selectRegion", Text = "<-- Select A Region First -->"});
           
            ViewBag.roadNames = LocationUpdate;

            //initialise a model for the index page
            IQueryable<QueryViewModel> initModel = trafficAccidentGateway.initModel();
            
            return View("Index", initModel);
        }

        [HttpPost]
        public ActionResult HandleForm(string regions, string roadNames, string period, string reportType)
        {
            
            List<SelectListItem> LocationUpdate = new List<SelectListItem>();
            List<SelectListItem> regionList = new List<SelectListItem>();

                //inserts the distinct regions from the database
            regionList.Add(new SelectListItem { Value = "Choose", Text = "<--  Region  -->" });
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }

            
            ViewBag.regions = regionList;
            //let the index page know what choices where made so as to display the right data for the user
            ViewBag.reportType = reportType;
            ViewBag.period = period;

            //find the roadnames based on the region chosen
            LocationUpdate.Add(new SelectListItem { Value = "Choose", Text = "<-- Select A Road Name -->" });
            var roadquery = (from ln in db.tblLocationNames
                             join rn in db.tblRoadNames on ln.lnID equals rn.rnLocation
                             where ln.lnRegion == regions
                             orderby rn.rnRoadName
                             select new { Text = rn.rnRoadName, Value = rn.rnRoadName }).Distinct().ToList();

            foreach (var item in roadquery)
            {
                LocationUpdate.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }
            ViewBag.roadNames = LocationUpdate;
            

            IQueryable<QueryViewModel> queryResults = trafficAccidentGateway.initModel();

            //filter the databases based on the choice of the user
            if (reportType.Equals("accident"))
            {
                queryResults = trafficAccidentGateway.filterDatabase(regions, roadNames, period, reportType);
                
            }
            else if (reportType.Equals("congestion"))
            {
                queryResults = trafficSpeedGateway.filterDatabase(regions, roadNames, period, reportType);
            }
           
            
            
            return View("Index", queryResults);
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

        [HttpGet]
        public ActionResult CallSecondaryOption(String id)
        {

            List<string> oneYearOption = new List<string>();
            for (int j = 16; j > 0; j--)
            {
                string i="20";
                if (j < 10) { i += "0"; }
                string str = i + j.ToString();
                oneYearOption.Add(str);
            }

            List<string> oneMonthOption = new List<string>();
            oneMonthOption.Add("January");
            oneMonthOption.Add("February");
            oneMonthOption.Add("March");
            oneMonthOption.Add("April");
            oneMonthOption.Add("May");
            oneMonthOption.Add("June");
            oneMonthOption.Add("July");
            oneMonthOption.Add("August");
            oneMonthOption.Add("September");
            oneMonthOption.Add("October");
            oneMonthOption.Add("November");
            oneMonthOption.Add("December");

            List<string> oneDayOption = new List<string>();
            for (int i=0; i<24; i++) {
                string str = i.ToString() + ":00";
                if (i < 10) { str = "0" + str; }
                oneDayOption.Add(str);
            }

            if ((id == "Day")) { return Json(oneDayOption, JsonRequestBehavior.AllowGet); }
            else if (id == "Month") { return Json(oneMonthOption, JsonRequestBehavior.AllowGet); }
            else if (id == "Year") { return Json(oneYearOption, JsonRequestBehavior.AllowGet); }
            
            else { return null; }
            
        }

        [HttpGet]
        public ActionResult CallRoadName(String region)
        {
            List<SelectListItem> LocationUpdate = new List<SelectListItem>();
            //List<string> LocationUpdate = new List<string>();
            var query = (from ln in db.tblLocationNames
                         join rn in db.tblRoadNames on ln.lnID equals rn.rnLocation
                         where ln.lnRegion == region
                         orderby rn.rnRoadName
                         select new { Text = rn.rnRoadName, Value = rn.rnRoadName }).Distinct().ToList();

            foreach (var item in query)
            {
                LocationUpdate.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }
            ViewBag.roadNames = LocationUpdate;
            //RedirectToAction("Index");
            //var query = (from ln in db.tblLocationNames
            //             orderby ln.lnRegion
            //             select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            //foreach (var item in query)
            //{
            //    regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            //}

            return Json(LocationUpdate, JsonRequestBehavior.AllowGet);
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
