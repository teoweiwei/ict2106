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
    public class TrafficSpeedsController : Controller
    {
        private TrafficReportContext db = new TrafficReportContext();

        // GET: TrafficSpeeds
        public ActionResult Index()
        {
            var tblTrafficSpeeds = db.tblTrafficSpeeds.Include(t => t.tblRoadName);
            return View(tblTrafficSpeeds.ToList());
        }

        // GET: TrafficSpeeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficSpeed tblTrafficSpeed = db.tblTrafficSpeeds.Find(id);
            if (tblTrafficSpeed == null)
            {
                return HttpNotFound();
            }
            return View(tblTrafficSpeed);
        }

        // GET: TrafficSpeeds/Create
        public ActionResult Create()
        {
            ViewBag.tsRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName");
            return View();
        }

        // POST: TrafficSpeeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tsID,tsDateTime,tsRoadName,tsMinSpeed,tsMaxSpeed")] tblTrafficSpeed tblTrafficSpeed)
        {
            if (ModelState.IsValid)
            {
                db.tblTrafficSpeeds.Add(tblTrafficSpeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tsRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficSpeed.tsRoadName);
            return View(tblTrafficSpeed);
        }

        // GET: TrafficSpeeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficSpeed tblTrafficSpeed = db.tblTrafficSpeeds.Find(id);
            if (tblTrafficSpeed == null)
            {
                return HttpNotFound();
            }
            ViewBag.tsRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficSpeed.tsRoadName);
            return View(tblTrafficSpeed);
        }

        // POST: TrafficSpeeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tsID,tsDateTime,tsRoadName,tsMinSpeed,tsMaxSpeed")] tblTrafficSpeed tblTrafficSpeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTrafficSpeed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tsRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficSpeed.tsRoadName);
            return View(tblTrafficSpeed);
        }

        // GET: TrafficSpeeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficSpeed tblTrafficSpeed = db.tblTrafficSpeeds.Find(id);
            if (tblTrafficSpeed == null)
            {
                return HttpNotFound();
            }
            return View(tblTrafficSpeed);
        }

        // POST: TrafficSpeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTrafficSpeed tblTrafficSpeed = db.tblTrafficSpeeds.Find(id);
            db.tblTrafficSpeeds.Remove(tblTrafficSpeed);
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
