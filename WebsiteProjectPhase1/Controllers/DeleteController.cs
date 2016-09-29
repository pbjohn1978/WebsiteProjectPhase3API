using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class DeleteController : Controller
    {
        // GET: Delete
        public ActionResult Index(int UserID)
        {
            //TODO: this has all kinds of problems... we will fix it tommrow... hehe
            if (!Models.SessionHelper.IsAdminSession())
                return RedirectToAction("Index", "Login");
            Models.SiteMember mem = Models.SessionHelper.GetMember();
            bool result = Models.HelperDB.DeleteMe(UserID);
            if (result)
                ViewBag.result = "the user has been deleted";
            else
                ViewBag.result = "there was an issue... the user has not been deleted";
            return View();
        }
    }
}