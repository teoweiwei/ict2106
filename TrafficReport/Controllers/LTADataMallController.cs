using System.Collections.Generic;
using System.Web.Mvc;
using TrafficReport.DAL;
using TrafficReport.Models;
using TrafficReport.Services;

namespace TrafficReport.Controllers
{
    //This controller handles user request on retrieving data from LTA DataMall web service
    public class LTADataMallController : Controller
    {
        private TrafficAccidentGateway trafficAccidentGateway = new TrafficAccidentGateway();
        private TrafficSpeedGateway trafficSpeedGateway = new TrafficSpeedGateway();
        private RoadNameGateway roadNameGateway = new RoadNameGateway();
        private ILTADataMallGateway ltaDataMallGateway = new LTADataMallGateway();

        //Index page
        public ActionResult index()
        {
            return View();
        }

        //Handle retrieving and saving of accident data
        public ActionResult GetAccidentData()
        {
            //Request for accident data from LTA web service
            List<LTADataMallModel.AccidentData> accidentData = ltaDataMallGateway.GetLTAAccidentData();

            //Save the retrieved records into database and display the listing to the view
            return View("SavedAccidentList", trafficAccidentGateway.SaveAccidentData(accidentData));
        }

        public ActionResult GetSpeedData()
        {
            //Request for speed data from LTA web service
            List<LTADataMallModel.SpeedData> speedData = ltaDataMallGateway.GetLTASpeedData();

            //Save the retrieved records into database and display the listing to the view
            return View("SavedSpeedList", trafficSpeedGateway.SaveSpeedData(speedData));
        }

        public ActionResult GetRoadNameData()
        {
            //Request for road names from LTA web service
            List<LTADataMallModel.SpeedData> roadNameData = ltaDataMallGateway.GetLTASpeedData();

            //Save the retrieved records into database and display the listing to the view
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
