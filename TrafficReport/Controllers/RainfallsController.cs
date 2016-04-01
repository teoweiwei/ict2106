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

            List<SelectListItem> regionList = new List<SelectListItem>();
            // List<myViewModel> regionList = new List<myViewModel>();
            var query = (from ln in db.tblLocationNames
                         orderby ln.lnRegion
                         select new { Text = ln.lnRegion, Value = ln.lnRegion }).Distinct().ToList();

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


            IQueryable<QueryViewModel> initModel = rainfallGateway.initModel();

            return View(initModel);
        }

        [HttpPost]
        public ActionResult HandleForm(string regions, string roadNames, string period)
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

            List<string> roadName = new List<string>();
            roadName.Add("Jurong East Avenue 1");
            roadName.Add("TAMPINES EXPRESSWAY");
            roadName.Add("PAN ISLAND EXPRESSWAY");
            roadName.Add("CENTRAL EXPRESSWAY");

            ViewData["roadNames"] = new SelectList(roadName);

            IQueryable<QueryViewModel> queryResults = trafficAccidentGateway.initModel();
            
            queryResults = rainfallGateway.filterDatabase(regions, roadNames, period);
            


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
