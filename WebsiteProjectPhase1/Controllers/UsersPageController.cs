using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class UsersPageController : Controller
    {
        // GET: UsersPage
        public ActionResult Index()
        {
            if (!Models.SessionHelper.IsMemberLoggedIn())
                return RedirectToAction("Index", "Login");
            Models.SiteMember mem = Models.SessionHelper.GetMember();
            
            return View(mem);
        }
    }
}