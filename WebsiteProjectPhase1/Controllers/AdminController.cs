using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteProjectPhase1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(int? id)
        {   
            ///this is the gaurd to keep out all people except admins
            ///ignore this line... its only a test comment... .. .
            ///new comment... 
            if(!Models.SessionHelper.IsAdminSession())
                return RedirectToAction("Index", "Login");
            //this gets the current users text color and sends it in a viewbag to the view
            Models.SiteMember mem = Models.SessionHelper.GetMember();
            ViewBag.txt = mem.TextColor;
            //this will get a list of sitemembers and send just 4 at a time to the view 
            //the number passed to the controler will determin what members get passed to the view next
            int pageNum = (id.HasValue) ? ((int)id)+1: 1;
            int memsPerPage = 4;
            List<Models.SiteMember> allMembers = Models.HelperDB.GetAllMembers();
            List<Models.SiteMember> mems = new List<Models.SiteMember>();
            int count = 0;
            foreach (var item in allMembers)
            {
                if (count < ((memsPerPage * pageNum) - memsPerPage))
                {
                    //skip: this represents the values for the previous pages
                }
                else if ( (count >= ((memsPerPage * pageNum) - memsPerPage)) && (count <= (memsPerPage * pageNum)) )
                {
                    mems.Add(item);
                }
                else
                {
                    //skip: this represents the values for the subsquent pages
                }   
                count++;
            }
            //this is named haha because i was messing around for like an hour trying to get a list of members to my view and 
            //this is the last methoud i found being used on the internets... AND I THOUGHT IT WOULD NOT WORK... but it did... 
            //so it got a bad viewbag name 'haha'
            ViewBag.haha = mems;
            ViewBag.pagenum = id;
            return View();
        }
    }
}