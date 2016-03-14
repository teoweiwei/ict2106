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

        // GET: Rainfalls
        public ActionResult Index()
        {
            return View();
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
