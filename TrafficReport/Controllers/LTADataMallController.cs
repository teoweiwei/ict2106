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
        private LTADataMallGateway ltaDataMallGateway = new LTADataMallGateway();

        public ActionResult index()
        {
            return View();
        }

        public ActionResult AccidentList()
        {
            return View(trafficAccidentGateway.SelectAll());
        }

        public ActionResult GetAccidentData()
        {
            List<LTADataMallModel.AccidentData> accidentData = ltaDataMallGateway.GetLTAAccidentData().d;

            return View("ConfirmAccidentList", trafficAccidentGateway.SaveAccidentData(accidentData));
        }



        public ActionResult GetSpeedData()
        {


            return View();
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
