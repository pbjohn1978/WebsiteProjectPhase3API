using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebsiteProjectPhase1.Models
{
    public class WeatherAPIhelper
    {
        public static string GetCurrentForecast()
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?zip=98498,us&APPID=904931765f13d1de295d1b562978fae2";
            WebClient syncClient = new WebClient();
            return syncClient.DownloadString(url);
        }
    }
}