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
    public class LocationNamesController : Controller
    {
        private LocationNameGateway locationNameGateway = new LocationNameGateway();

        // GET: LocationNames
        public ActionResult Index()
        {
            return View(locationNameGateway.SelectAll());
        }

        // GET: LocationNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = locationNameGateway.SelectById(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // GET: LocationNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lnID,lnLocationName,lnRegion")] tblLocationName tblLocationName)
        {
            if (ModelState.IsValid)
            {
                locationNameGateway.Insert(tblLocationName);
                return RedirectToAction("Index");
            }

            return View(tblLocationName);
        }

        // GET: LocationNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = locationNameGateway.SelectById(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // POST: LocationNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lnID,lnLocationName,lnRegion")] tblLocationName tblLocationName)
        {
            if (ModelState.IsValid)
            {
                locationNameGateway.Update(tblLocationName);
                return RedirectToAction("Index");
            }
            return View(tblLocationName);
        }

        // GET: LocationNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLocationName tblLocationName = locationNameGateway.SelectById(id);
            if (tblLocationName == null)
            {
                return HttpNotFound();
            }
            return View(tblLocationName);
        }

        // POST: LocationNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            locationNameGateway.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                locationNameGateway.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
