using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.SiteMember mem = Models.HelperDB.GetMember(model);
                if (mem == null)
                {
                    ModelState.AddModelError("InvalidCredentials", "No user with those credentials was found.");
                    return RedirectToAction("Index", "Home");
                }
                Session["MemberName"] = mem.Username;
                return RedirectToAction("Index", "UsersPage");
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}