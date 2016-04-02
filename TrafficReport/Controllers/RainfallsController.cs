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
    public class RainfallsController : Controller
    {
        private RainfallGateway rainfallGateway = new RainfallGateway();
        private TrafficReportContext db = new TrafficReportContext();

        private TrafficAccidentGateway trafficAccidentGateway = new TrafficAccidentGateway();

        // GET: Rainfalls
        public ActionResult Index()
        {
            
            List<SelectListItem> LocationUpdate = new List<SelectListItem>();
            List<SelectListItem> regionList = new List<SelectListItem>();

            regionList.Add(new SelectListItem { Value = "Choose", Text = "<--  Region  -->" });
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }


            ViewBag.regions = regionList;

            LocationUpdate.Add(new SelectListItem { Value = "selectRegion", Text = "<-- Select A Region First -->" });

            ViewBag.roadNames = LocationUpdate;
            


            IQueryable<QueryViewModel> initModel = rainfallGateway.InitModel();

            return View(initModel);
        }

        [HttpPost]
        public ActionResult HandleForm(string regions, string roadNames, string period)
        {
            List<SelectListItem> LocationUpdate = new List<SelectListItem>();
            List<SelectListItem> regionList = new List<SelectListItem>();

            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

            foreach (var item in query)
            {
                regionList.Add(new SelectListItem { Value = item.Value.ToString(), Text = item.Text });
            }

            
            ViewBag.regions = regionList;
            //let the index page know what choices where made so as to display the right data for the user
            
            ViewBag.period = period;

            LocationUpdate.Add(new SelectListItem { Value = "Choose", Text = "<-- Select A Road Name -->" });

            //find the roadnames based on the region chosen
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


            IQueryable<QueryViewModel> queryResults = trafficAccidentGateway.InitModel();
            
            //filter the rainfall based on the search query
            queryResults = rainfallGateway.FilterDatabase(regions, roadNames, period);
            


            return View("Index", queryResults);
        }

        public ActionResult Upload()
        {
            return View(rainfallGateway.SelectAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<tblRainfall> savedRainfallRecord = rainfallGateway.SaveRainfallData(upload);

                if(savedRainfallRecord == null)
                {
                    ModelState.AddModelError("File", "Error in uploading file");
                    return View("Index");
                }
                else
                {
                    return View(savedRainfallRecord);
                }
            }
            else
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("Index");
            }
        }
    }
}
