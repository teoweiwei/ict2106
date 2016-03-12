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
using TrafficReport.Services;

namespace TrafficReport.Controllers
{
    public class RoadNamesController : Controller
    {
        private RoadNameGateway roadNameGateway = new RoadNameGateway();
        private LTADataMallGateway ltaDataMallGateway = new LTADataMallGateway();
        //List<LTADataMallModel.SpeedData> speedData = ltaDataMallGateway.GetLTASpeedData().d;
        // GET: RoadNames
        public ActionResult Index()
        {
            int skipCount = 0;
            List<LTADataMallModel.SpeedData> speedData = ltaDataMallGateway.GetLTASpeedData().d;


            do
            {
                for(int i=0; i<speedData.Count(); i++)
                {
                    tblRoadName roadName = new tblRoadName();

                    roadName.rnID = speedData[i].LinkID;
                    roadName.rnRoadName = speedData[i].RoadName;
                    roadName.rnSpeedLimit = 50;

                    tblRoadName check = roadNameGateway.SelectById(speedData[i].LinkID);
                    Boolean count = (check == null);

                    IEnumerable<tblRoadName> check2 = roadNameGateway.db.tblRoadNames.Where(m => m.rnRoadName.Equals(roadName.rnRoadName)).ToList();


                    Boolean count2 = (check2.Count() == 0);

                    if (count && count2)
                    {
                        roadNameGateway.Insert(roadName);
                    }
                }

                skipCount += 50;
                speedData = ltaDataMallGateway.GetLTARoadName(skipCount).d;

                if(!(speedData.Count() == 0))
                {
                    int w = 0;
                }
            } while (!(speedData.Count() == 0)); 
            


            return View("List", speedData);

            // var tblRoadNames = db.tblRoadNames.Include(t => t.tblLocationName);
            // return View(tblRoadNames.ToList());
        }

/*        // GET: RoadNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoadName tblRoadName = db.tblRoadNames.Find(id);
            if (tblRoadName == null)
            {
                return HttpNotFound();
            }
            return View(tblRoadName);
        }

        // GET: RoadNames/Create
        public ActionResult Create()
        {
            ViewBag.rnLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName");
            return View();
        }

        // POST: RoadNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rnID,rnRoadName,rnLocation,rnSpeedLimit")] tblRoadName tblRoadName)
        {
            if (ModelState.IsValid)
            {
                db.tblRoadNames.Add(tblRoadName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rnLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRoadName.rnLocation);
            return View(tblRoadName);
        }

        // GET: RoadNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoadName tblRoadName = db.tblRoadNames.Find(id);
            if (tblRoadName == null)
            {
                return HttpNotFound();
            }
            ViewBag.rnLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRoadName.rnLocation);
            return View(tblRoadName);
        }

        // POST: RoadNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rnID,rnRoadName,rnLocation,rnSpeedLimit")] tblRoadName tblRoadName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRoadName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rnLocation = new SelectList(db.tblLocationNames, "lnID", "lnLocationName", tblRoadName.rnLocation);
            return View(tblRoadName);
        }

        // GET: RoadNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoadName tblRoadName = db.tblRoadNames.Find(id);
            if (tblRoadName == null)
            {
                return HttpNotFound();
            }
            return View(tblRoadName);
        }

        // POST: RoadNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRoadName tblRoadName = db.tblRoadNames.Find(id);
            db.tblRoadNames.Remove(tblRoadName);
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
        }*/
    }
}
