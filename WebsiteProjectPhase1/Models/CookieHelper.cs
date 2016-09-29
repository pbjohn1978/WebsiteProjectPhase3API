using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteProjectPhase1.Models
{
    public class CookieHelper
    {
        public static void CreateCookie(string user, string code, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(user, code);
            cookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}