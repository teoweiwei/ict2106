using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrafficReport.Models;

namespace TrafficReport.Controllers
{
    public class RainfallsController : Controller
    {
        private trafficreportEntities db = new trafficreportEntities();

        // GET: Rainfalls
        public ActionResult Index()
        {
            var tblRainfalls = db.tblRainfalls.Include(t => t.tblLocationName);
            return View(tblRainfalls.ToList());
        }

        // GET: Rainfalls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRainfall tblRainfall = db.tblRainfalls.Find(id);
            if (tblRainfall == null)
            {
                return HttpNotFound();
            }
            return View(tblRainfall);
        }

        // GET: Rainfalls/Create
        public ActionResult Create()
        {
            ViewBag.rfLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName");
            return View();
        }

        // POST: Rainfalls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rfID,rfDate,rfLocation,rfValue")] tblRainfall tblRainfall)
        {
            if (ModelState.IsValid)
            {
                db.tblRainfalls.Add(tblRainfall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rfLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRainfall.rfLocation);
            return View(tblRainfall);
        }

        // GET: Rainfalls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRainfall tblRainfall = db.tblRainfalls.Find(id);
            if (tblRainfall == null)
            {
                return HttpNotFound();
            }
            ViewBag.rfLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRainfall.rfLocation);
            return View(tblRainfall);
        }

        // POST: Rainfalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rfID,rfDate,rfLocation,rfValue")] tblRainfall tblRainfall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRainfall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rfLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRainfall.rfLocation);
            return View(tblRainfall);
        }

        // GET: Rainfalls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRainfall tblRainfall = db.tblRainfalls.Find(id);
            if (tblRainfall == null)
            {
                return HttpNotFound();
            }
            return View(tblRainfall);
        }

        // POST: Rainfalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRainfall tblRainfall = db.tblRainfalls.Find(id);
            db.tblRainfalls.Remove(tblRainfall);
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
