using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteProjectPhase1.Models
{
    public static class ValidateMemberData
    {   
        /// <summary>
        /// this methoud takes in a sitememember object and returns a validated sitemember object
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static SiteMember IsValid(SiteMember NewMemData)
        {
            //chunk of code below strips out white space in front and back of the users input
            if (NewMemData.DesplayMessage != null)
                NewMemData.DesplayMessage = NewMemData.DesplayMessage.Trim();
            if (NewMemData.TextColor != null)
                NewMemData.TextColor = NewMemData.TextColor.Trim();
            if (NewMemData.BackgroundColor != null)
                NewMemData.BackgroundColor = NewMemData.BackgroundColor.Trim();
            if (NewMemData.ConfirmPassword != null)
                NewMemData.ConfirmPassword = NewMemData.ConfirmPassword.Trim();
            if (NewMemData.Password != null)
                NewMemData.Password = NewMemData.Password.Trim();
            if (NewMemData.Username != null)
                NewMemData.Username = NewMemData.Username.Trim();

            //this chunk of code will input default values for the bgcolor, txtcolor and display message 
            //those values can accept nulls from the user Registor page...
            if (NewMemData.BackgroundColor == null)
                if (NewMemData.TextColor == "white")
                    NewMemData.BackgroundColor = "black";
                else
                    NewMemData.BackgroundColor = "white";
            if (NewMemData.TextColor == null)
                if (NewMemData.BackgroundColor == "black")
                    NewMemData.TextColor = "white";
                else
                    NewMemData.TextColor = "black";
            if (NewMemData.DesplayMessage == null)
                NewMemData.DesplayMessage = $"hello, {NewMemData.Username}";

            //the following two if statements will change the users inputted colors to the default values of black text/white background
            //if the user entered a color into one of the two color input txt boxes that is not in the database
            if (Models.HelperDB.IsBackGroundColorInDB(NewMemData) == false)
            {
                if (NewMemData.TextColor != "white")
                    NewMemData.BackgroundColor = "white";
                else
                    NewMemData.BackgroundColor = "black";
            }
            if (Models.HelperDB.IsTextColorInDB(NewMemData) == false)
            {
                if (NewMemData.BackgroundColor != "black")
                    NewMemData.TextColor = "black";
                else
                    NewMemData.TextColor= "white";
            }

            //this will fix the users colors if they put the same two colors in for both background and text colors
            if (NewMemData.TextColor == NewMemData.BackgroundColor)
            {
                if (NewMemData.TextColor == "black")
                    NewMemData.BackgroundColor = "white";
                else
                    NewMemData.TextColor = "black";
            }


            return NewMemData;
        }
    }
}