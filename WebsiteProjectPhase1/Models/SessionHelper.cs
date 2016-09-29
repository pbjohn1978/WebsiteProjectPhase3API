using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteProjectPhase1.Models
{
    public class SessionHelper
    {
        public static bool IsMemberLoggedIn()
        {
            if (HttpContext.Current.Session["MemberName"] == null)
                return false;
            return true;
        }

        public static Models.SiteMember GetMember()
        {
            SiteMember mem = Models.HelperDB.GetMember(HttpContext.Current.Session["MemberName"].ToString());
            return mem;
        }

        public static bool IsAdminSession()
        {
            if (HttpContext.Current.Session["MemberName"] != null)
            {
                if (Models.HelperDB.GetMember(HttpContext.Current.Session["MemberName"].ToString()).AccessLevel > 1)
                    return true;
            }
            return false;
        }
    }
}