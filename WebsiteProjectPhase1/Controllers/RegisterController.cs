using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.SiteMember NewMemData)
        {
            //this will check to see if the username has been added to the database already...
            if (Models.HelperDB.IsUserNameTaken(NewMemData))
            {
                TempData["ErrorMessage"] = "Username has been taken";
                return RedirectToAction("Index", "Error");
            }
            if (NewMemData.ConfirmPassword != NewMemData.Password)
            {
                TempData["ErrorMessage"] = "The Password and Confirm Password did not match, you were not added to the site.";
                return RedirectToAction("Index", "Error");
            }
            //this line will valid everything except the username (I am doing that in the line above)
            Models.SiteMember ValidMemberData = Models.ValidateMemberData.IsValid(NewMemData);

            bool isAdded = Models.HelperDB.AddMemberToDB(ValidMemberData);
            if (isAdded)
            {
                return RedirectToAction("Index", "Success");///this is to the success message page
            }
            else
            {
                //general error message... dont know what went wrong but something did...
                TempData["ErrorMessage"] = @"There was a problem with the database... I was not able to add you to our system... 
Please Try again soon... .. . I swar it's me not you... .. . sorry :( :( :(";
                return RedirectToAction("Index", "Error");
            }
        }
        
        [HttpGet]
        public ActionResult ModifyUserInfo()
        {
            if (!Models.SessionHelper.IsMemberLoggedIn())
                return RedirectToAction("Index", "Login");
            Models.SiteMember mem = Models.SessionHelper.GetMember();
            return View(mem);
        }

        [HttpPost]
        public ActionResult ModifyUserInfo(Models.SiteMember modifiedUserData)
        {
            if (modifiedUserData.Username != Models.SessionHelper.GetMember().Username && Models.HelperDB.IsUserNameTaken(modifiedUserData))
            {
                TempData["ErrorMessage"] = "Username has been taken";
                return RedirectToAction("Index", "Error");
            }
            if (modifiedUserData.ConfirmPassword != modifiedUserData.Password)
            {
                TempData["ErrorMessage"] = "The Password and Confirm Password did not match, you were not added to the site.";
                return RedirectToAction("Index", "Error");
            }
            //this line will valid everything except the username (I am doing that in the line above)
            Models.SiteMember ValidMemberData = Models.ValidateMemberData.IsValid(modifiedUserData);
            bool isAdded = Models.HelperDB.UpdateUser(modifiedUserData);
            if (isAdded)
            {
                return RedirectToAction("Index", "Success");///this is to the success message page
            }
            else
            {
                //general error message... dont know what went wrong but something did...
                TempData["ErrorMessage"] = @"There was a problem with the database... I was not able to add you to our system... 
Please Try again soon... .. . I swar it's me not you... .. . sorry :( :( :(";
                return RedirectToAction("Index", "Error");
            }
        }
    }
}