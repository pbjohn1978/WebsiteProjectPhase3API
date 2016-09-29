using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteProjectPhase1.Models;

namespace WebsiteProjectPhase1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string result = Models.WeatherAPIhelper.GetCurrentForecast();
            WeatherData weather = JsonConvert.DeserializeObject<WeatherData>(result);
            

            ViewBag.weather = weather;
            return View();
        }
    }
}