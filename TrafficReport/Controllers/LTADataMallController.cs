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
    public class LTADataMallController : Controller
    {
        private TrafficAccidentGateway trafficAccidentGateway = new TrafficAccidentGateway();
        private TrafficSpeedGateway trafficSpeedGateway = new TrafficSpeedGateway();
        private RoadNameGateway roadNameGateway = new RoadNameGateway();
        private LTADataMallGateway ltaDataMallGateway = new LTADataMallGateway();

        public ActionResult index()
        {
            return View();
        }

        public ActionResult AccidentList()
        {
            return View(trafficAccidentGateway.SelectAll());
        }

        public ActionResult SpeedList()
        {
            return View(trafficAccidentGateway.SelectAll());
        }

        //public ActionResult GetAccidentData()
        //{
        //    List<LTADataMallModel.AccidentData> accidentData = ltaDataMallGateway.GetLTAAccidentData();
        //    return View("SavedAccidentList", trafficAccidentGateway.SaveAccidentData(accidentData));
        //}

        public ActionResult GetSpeedData()
        {
            List<LTADataMallModel.SpeedData> speedData = ltaDataMallGateway.GetLTASpeedData();
            return View("SavedSpeedList", trafficSpeedGateway.SaveSpeedData(speedData));
        }

        public ActionResult GetRoadNameData()
        {
            List<LTADataMallModel.SpeedData> roadNameData = ltaDataMallGateway.GetLTASpeedData();
            return View("SavedRoadNameList", roadNameGateway.SaveRoadNameData(roadNameData));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trafficAccidentGateway.db.Dispose();
                trafficSpeedGateway.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
