using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class CookieMakerController : Controller
    {
        // GET: CookieMaker
        public ActionResult Index()
        {
            if (!Models.SessionHelper.IsMemberLoggedIn())
                return RedirectToAction("Index", "Login");
            Models.SiteMember mem = Models.SessionHelper.GetMember();
            string randomNess = Guid.NewGuid().ToString();
            mem.CodeForNoPwLogin = randomNess;
            bool isUpdated = Models.HelperDB.addCookieValueToDB(mem);
            //TODO: if you have time before midnight do something with the returned bool...
            Models.CookieHelper.CreateCookie("user", randomNess, DateTime.Now.AddYears(300) );
            return View();
        }
    }
}