﻿using System;
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
    public class TrafficAccidentsController : Controller
    {
        private TrafficReportContext db = new TrafficReportContext();

        // GET: TrafficAccidents
        public ActionResult Index()
        {
            var tblTrafficAccidents = db.tblTrafficAccidents.Include(t => t.tblRoadName);
            return View(tblTrafficAccidents.ToList());
        }

        // GET: TrafficAccidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficAccident tblTrafficAccident = db.tblTrafficAccidents.Find(id);
            if (tblTrafficAccident == null)
            {
                return HttpNotFound();
            }
            return View(tblTrafficAccident);
        }

        // GET: TrafficAccidents/Create
        public ActionResult Create()
        {
            ViewBag.taRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName");
            return View();
        }

        // POST: TrafficAccidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taID,taDateTime,taRoadName,taLat,taLong,taDescription")] tblTrafficAccident tblTrafficAccident)
        {
            if (ModelState.IsValid)
            {
                db.tblTrafficAccidents.Add(tblTrafficAccident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.taRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficAccident.taRoadName);
            return View(tblTrafficAccident);
        }

        // GET: TrafficAccidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficAccident tblTrafficAccident = db.tblTrafficAccidents.Find(id);
            if (tblTrafficAccident == null)
            {
                return HttpNotFound();
            }
            ViewBag.taRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficAccident.taRoadName);
            return View(tblTrafficAccident);
        }

        // POST: TrafficAccidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taID,taDateTime,taRoadName,taLat,taLong,taDescription")] tblTrafficAccident tblTrafficAccident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTrafficAccident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.taRoadName = new SelectList(db.tblRoadNames, "rnID", "rnRoadName", tblTrafficAccident.taRoadName);
            return View(tblTrafficAccident);
        }

        // GET: TrafficAccidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTrafficAccident tblTrafficAccident = db.tblTrafficAccidents.Find(id);
            if (tblTrafficAccident == null)
            {
                return HttpNotFound();
            }
            return View(tblTrafficAccident);
        }

        // POST: TrafficAccidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTrafficAccident tblTrafficAccident = db.tblTrafficAccidents.Find(id);
            db.tblTrafficAccidents.Remove(tblTrafficAccident);
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
