using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterJugChallenge.Models;

namespace WaterJugChallenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getWaterChallengeSolution(int JugA, int JugB, int JugZ )
        {
            WaterChallenge waterChallenge = new Models.WaterChallenge(JugA, JugB, JugZ);
            var actions = waterChallenge.GetActionsSolution();
            return Json(actions, JsonRequestBehavior.AllowGet);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}