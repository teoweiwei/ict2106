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
            ViewBag.findByDay = "0";
        }
        // GET: Traffic
        public ActionResult Index(string val)
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
            roadName.Add("TAMPINES EXPRESSWAY");
            roadName.Add("PAN ISLAND EXPRESSWAY");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);


            IQueryable<QueryViewModel> initModel = DataGateway.initModel();
            
            return View(initModel);
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
            ViewBag.reportType = reportType;
            ViewBag.period = period;


            List < string> roadName = new List<string>();
            roadName.Add("Jurong East Avenue 1");
            roadName.Add("TAMPINES EXPRESSWAY");
            roadName.Add("PAN ISLAND EXPRESSWAY");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);

            IQueryable<QueryViewModel> queryResults = trafficAccidentGateway.initModel();
            //IEnumerable<QueryViewModel> queryR = trafficAccidentGateway.filterDatabase(regions, roadNames, period, reportType); 

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
        public ActionResult CallSecondaryOption(char id)
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

            if ((id == '1')) { return Json(oneDayOption, JsonRequestBehavior.AllowGet); }
            else if (id == '2') { return Json(oneMonthOption, JsonRequestBehavior.AllowGet); }
            else if (id == '3') { return Json(oneYearOption, JsonRequestBehavior.AllowGet); }
            else { return null; }
            
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
