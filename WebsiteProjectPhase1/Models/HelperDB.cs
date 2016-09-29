using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebsiteProjectPhase1.Models
{
    public static class HelperDB
    {
        /// <summary>
        /// this adds a new member to the db
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        public static bool AddMemberToDB(SiteMember mem)
        {
            ///*****************************************************************************************************************************
            /// important- the line below will automatically give ALL new members a NON admin account... If you wanna be an admin this 
            /// site will NOT add an admin, this must be accomplished through management studio... This is a security feature... :)
            ///*****************************************************************************************************************************
            if (!Models.SessionHelper.IsMemberLoggedIn())
                mem.AccessLevel = 1;
            else
                mem.AccessLevel = Models.SessionHelper.GetMember().AccessLevel;

            bool IsAdded = false;

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand regMember = new SqlCommand();

            regMember.Connection = con;

            regMember.CommandText = @"INSERT INTO [dbo].[WebSiteMembers]([MemberName],[MemberPassword] ,[MemberBGcolor] ,[MemberTextColor],[MemberText],[MemberAccess]) VALUES( @username , @password , @bgcolor , @txtcolor , @dismessage , @access )";
            
            regMember.Parameters.AddWithValue("@bgcolor", mem.BackgroundColor);
            regMember.Parameters.AddWithValue("@dismessage", mem.DesplayMessage);
            regMember.Parameters.AddWithValue("@txtcolor", mem.TextColor);
            regMember.Parameters.AddWithValue("@username", mem.Username);
            regMember.Parameters.AddWithValue("@password", mem.Password);
            regMember.Parameters.AddWithValue("@access", mem.AccessLevel);
            
            try
            {
                con.Open();
                int rows = regMember.ExecuteNonQuery();
                if (rows == 1)
                {
                    IsAdded = true;
                }
            }
            catch
            {
                IsAdded = false;
            }
            finally
            {
                con.Dispose();
            }
            return IsAdded;
        }


        /// <summary>
        /// this decides if the username is taken or not... :)
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        public static bool IsUserNameTaken(SiteMember mem)
        {
            bool isUserInDB = false;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isMember = new SqlCommand();
            isMember.Connection = con;

            isMember.CommandText = @"SELECT *
  FROM [dbo].[WebSiteMembers]
where @username = [MemberName]";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isMember.Parameters.AddWithValue("@username", mem.Username);

            try
            {
                con.Open();
                SqlDataReader reader = isMember.ExecuteReader();
                if (reader.HasRows)
                {
                    isUserInDB = true;
                }
            }
            finally
            {
                con.Dispose();
            }
            return isUserInDB;
        }


        /// <summary>
        /// this methoud will return TRUE if the color is in the db as a valid color 
        /// returns false if the color is NOT in the db... therefor the inputed color by the user is NOT valid
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        public static bool IsBackGroundColorInDB(SiteMember mem)
        {
            bool IsColorinDB = false;

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isColor = new SqlCommand();

            isColor.Connection = con;

            isColor.CommandText = @"SELECT [Color]
  FROM [dbo].[colors]
where @bgcolor = Color";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isColor.Parameters.AddWithValue("@bgcolor", mem.BackgroundColor);

            try
            {
                con.Open();
                SqlDataReader reader = isColor.ExecuteReader();
                if (reader.HasRows)
                {
                    IsColorinDB = true;
                }
            }
            finally
            {
                con.Dispose();
            }
            return IsColorinDB;
        }


        /// <summary>
        /// this methoud will return TRUE if the color is in the db as a valid color 
        /// returns false if the color is NOT in the db... therefor the inputed color by the user is NOT valid
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        public static bool IsTextColorInDB(SiteMember mem)
        {
            bool IsColorinDB = false;

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isColor = new SqlCommand();

            isColor.Connection = con;

            isColor.CommandText = @"SELECT [Color]
  FROM [dbo].[colors]
where @txtcolor = Color";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isColor.Parameters.AddWithValue("@txtcolor", mem.TextColor);

            try
            {
                con.Open();
                SqlDataReader reader = isColor.ExecuteReader();
                if (reader.HasRows)
                {
                    IsColorinDB = true;
                }
            }
            finally
            {
                con.Dispose();
            }
            return IsColorinDB;
        }


        /// <summary>
        /// This will take in a LoginInViewModel object and return a SiteMember object if they are already registered if they are NOT in the db it will return NULL
        /// </summary>
        /// <param name="nameAndPass"></param>
        /// <returns></returns>
        public static Models.SiteMember GetMember(Models.LoginViewModel nameAndPass)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isInDb = new SqlCommand();

            isInDb.Connection = con;

            isInDb.CommandText = @"SELECT [MemberName]
      ,[MemberPassword]
      ,[MemberBGcolor]
      ,[MemberTextColor]
      ,[MemberText]
      ,[MemberAccess]
      ,[Member_ID]
  FROM [dbo].[WebSiteMembers]
  where @ps = [MemberPassword] and @un = [MemberName]";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isInDb.Parameters.AddWithValue("@ps", nameAndPass.Password);
            isInDb.Parameters.AddWithValue("@un", nameAndPass.Username);

            try
            {
                con.Open();
                SqlDataReader reader = isInDb.ExecuteReader();
                if (reader.Read())
                {
                    SiteMember mem = new SiteMember();
                    mem.BackgroundColor = reader["MemberBGcolor"].ToString();
                    mem.Username = reader["MemberName"].ToString();
                    mem.Password = reader["MemberPassword"].ToString();
                    mem.ConfirmPassword = reader["MemberPassword"].ToString();
                    mem.AccessLevel = Convert.ToByte(reader["MemberAccess"]);
                    mem.TextColor = reader["MemberTextColor"].ToString();
                    mem.DesplayMessage = reader["MemberText"].ToString();
                    mem.UserID = Convert.ToInt32(reader["Member_ID"]);
                    return mem;
                }
                else
                    return null;
            }
            finally
            {
                con.Dispose();
            }
        }


        /// <summary>
        /// this will take in a USERID and return a SiteMember object
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Models.SiteMember GetMember(string username)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isInDb = new SqlCommand();

            isInDb.Connection = con;

            isInDb.CommandText = @"SELECT [MemberName]
      ,[MemberPassword]
      ,[MemberBGcolor]
      ,[MemberTextColor]
      ,[MemberText]
      ,[MemberAccess]
      ,[Member_ID]
  FROM [dbo].[WebSiteMembers]
  where @user = [MemberName]";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isInDb.Parameters.AddWithValue("@user", username);

            try
            {
                con.Open();
                SqlDataReader reader = isInDb.ExecuteReader();
                if (reader.Read())
                {
                    SiteMember mem = new SiteMember();
                    mem.BackgroundColor = reader["MemberBGcolor"].ToString();
                    mem.Username = reader["MemberName"].ToString();
                    mem.Password = reader["MemberPassword"].ToString();
                    mem.ConfirmPassword = reader["MemberPassword"].ToString();
                    mem.AccessLevel = Convert.ToByte(reader["MemberAccess"]);
                    mem.TextColor = reader["MemberTextColor"].ToString();
                    mem.DesplayMessage = reader["MemberText"].ToString();
                    mem.UserID = Convert.ToInt32(reader["Member_ID"]);
                    return mem;
                }
                else
                    return null;
            }
            finally
            {
                con.Dispose();
            }
        }


        /// <summary>
        /// this methoud takes in a SiteMember and updates there informations based on their MemberID in the db.
        /// </summary>
        /// <param name="UpdateMePweeze"></param>
        /// <returns></returns>
        public static bool UpdateUser(Models.SiteMember UpdateMePweeze)
        {
            bool isUpdated = false;

            UpdateMePweeze.AccessLevel = Models.SessionHelper.GetMember().AccessLevel;
            UpdateMePweeze.UserID = Models.SessionHelper.GetMember().UserID;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand regMember = new SqlCommand();

            regMember.Connection = con;

            regMember.CommandText = @"UPDATE [dbo].[WebSiteMembers]
   SET [MemberName] = @name
      ,[MemberPassword] = @pass
      ,[MemberBGcolor] = @bgc
      ,[MemberTextColor] = @txc
      ,[MemberText] = @txt
      ,[MemberAccess] = @access
 WHERE Member_ID = @idnum";

            regMember.Parameters.AddWithValue("@bgc", UpdateMePweeze.BackgroundColor);
            regMember.Parameters.AddWithValue("@txt", UpdateMePweeze.DesplayMessage);
            regMember.Parameters.AddWithValue("@txc", UpdateMePweeze.TextColor);
            regMember.Parameters.AddWithValue("@name", UpdateMePweeze.Username);
            regMember.Parameters.AddWithValue("@pass", UpdateMePweeze.Password);
            regMember.Parameters.AddWithValue("@access", UpdateMePweeze.AccessLevel);
            regMember.Parameters.AddWithValue("@idnum", UpdateMePweeze.UserID);

            try
            {
                con.Open();
                int rows = regMember.ExecuteNonQuery();
                if (rows == 1)
                {
                    isUpdated = true;
                }
            }
            catch
            {
                isUpdated = false;
            }
            finally
            {
                con.Dispose();
            }
            return isUpdated;
        }


        /// <summary>
        /// this will get all the sitemembers and there data and put it in a List
        /// </summary>
        /// <returns></returns>
        public static List<Models.SiteMember> GetAllMembers()
        {
            List<Models.SiteMember> members = new List<SiteMember>();

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isColor = new SqlCommand();

            isColor.Connection = con;

            isColor.CommandText = @"SELECT [Member_ID]
      ,[MemberName]
      ,[MemberPassword]
      ,[MemberBGcolor]
      ,[MemberTextColor]
      ,[MemberText]
      ,[MemberAccess]
  FROM [WebSiteMember].[dbo].[WebSiteMembers] ";

            try
            {
                con.Open();
                SqlDataReader reader = isColor.ExecuteReader();
                while (reader.Read())
                {
                    SiteMember temp = new SiteMember();
                    temp.UserID = reader.GetInt32(0);
                    temp.Username = reader.GetString(1);
                    temp.Password = reader.GetString(2);
                    temp.ConfirmPassword = reader.GetString(2);
                    temp.BackgroundColor = reader.GetString(3);
                    temp.TextColor = reader.GetString(4);
                    temp.DesplayMessage = reader.GetString(5);
                    temp.AccessLevel = reader.GetByte(6);
                    members.Add(temp);
                }
            }
            finally
            {
                con.Dispose();
            }
            return members;
        }


        /// <summary>
        /// this takes in an interger value representing the UserID and will delete the user from the database.
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns></returns>
        public static bool DeleteMe(int idnum)
        {
            bool isDeleted = false;

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand regMember = new SqlCommand();

            regMember.Connection = con;

            regMember.CommandText = @"DELETE FROM [dbo].[WebSiteMembers]
      WHERE Member_ID = @idnum";

            regMember.Parameters.AddWithValue("@idnum", idnum);

            try
            {
                con.Open();
                int rows = regMember.ExecuteNonQuery();
                if (rows == 1)
                {
                    isDeleted = true;
                }
            }
            catch
            {
                isDeleted = false;
            }
            finally
            {
                con.Dispose();
            }
            return isDeleted;

        }


        /// <summary>
        /// this will add the GUID Value to the DB
        /// </summary>
        /// <returns></returns>
        public static bool addCookieValueToDB(Models.SiteMember UpdateMePweeze)
        {
            bool isUpdated = false;

            UpdateMePweeze.AccessLevel = Models.SessionHelper.GetMember().AccessLevel;
            UpdateMePweeze.UserID = Models.SessionHelper.GetMember().UserID;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand regMember = new SqlCommand();

            regMember.Connection = con;

            regMember.CommandText = @"UPDATE [dbo].[WebSiteMembers]
   SET [MemberName] = @name
      ,[MemberPassword] = @pass
      ,[MemberBGcolor] = @bgc
      ,[MemberTextColor] = @txc
      ,[MemberText] = @txt
      ,[MemberAccess] = @access
      ,[GuidGoesHereJohn] = @code
 WHERE Member_ID = @idnum";

            regMember.Parameters.AddWithValue("@code", UpdateMePweeze.CodeForNoPwLogin);
            regMember.Parameters.AddWithValue("@bgc", UpdateMePweeze.BackgroundColor);
            regMember.Parameters.AddWithValue("@txt", UpdateMePweeze.DesplayMessage);
            regMember.Parameters.AddWithValue("@txc", UpdateMePweeze.TextColor);
            regMember.Parameters.AddWithValue("@name", UpdateMePweeze.Username);
            regMember.Parameters.AddWithValue("@pass", UpdateMePweeze.Password);
            regMember.Parameters.AddWithValue("@access", UpdateMePweeze.AccessLevel);
            regMember.Parameters.AddWithValue("@idnum", UpdateMePweeze.UserID);

            try
            {
                con.Open();
                int rows = regMember.ExecuteNonQuery();
                if (rows == 1)
                {
                    isUpdated = true;
                }
            }
            catch
            {
                isUpdated = false;
            }
            finally
            {
                con.Dispose();
            }
            return isUpdated;
        }


        /// <summary>
        /// this takes in a string and will return true if the String Value from the cookie is a Valid code in the DB
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsCookieValid(string code)
        {
            bool isValid = false;

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isColor = new SqlCommand();

            isColor.Connection = con;

            isColor.CommandText = @"SELECT [GuidGoesHereJohn]
  FROM [dbo].[WebSiteMembers]
  where GuidGoesHereJohn = @code";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isColor.Parameters.AddWithValue("@code", code);

            try
            {
                con.Open();
                SqlDataReader reader = isColor.ExecuteReader();
                if (reader.HasRows)
                {
                    isValid = true;
                }
            }
            finally
            {
                con.Dispose();
            }

            return isValid;
        }


        /// <summary>
        /// this takes in a string and will return a full Sitemember Object...
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SiteMember GetSiteMemFromCookieCode(string code)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MEMBERDB"].ConnectionString);

            SqlCommand isInDb = new SqlCommand();

            isInDb.Connection = con;

            isInDb.CommandText = @"SELECT [MemberName]
      ,[MemberPassword]
      ,[MemberBGcolor]
      ,[MemberTextColor]
      ,[MemberText]
      ,[MemberAccess]
      ,[Member_ID]
      ,[GuidGoesHereJohn]
  FROM [dbo].[WebSiteMembers]
  where @code = [GuidGoesHereJohn] ";

            //username and password are required so they will always have values.
            //the if else blocks will insert default data in for the user if none is entered. :)
            isInDb.Parameters.AddWithValue("@code", code);

            try
            {
                con.Open();
                SqlDataReader reader = isInDb.ExecuteReader();
                if (reader.Read())
                {
                    SiteMember mem = new SiteMember();
                    mem.BackgroundColor = reader["MemberBGcolor"].ToString();
                    mem.Username = reader["MemberName"].ToString();
                    mem.Password = reader["MemberPassword"].ToString();
                    mem.ConfirmPassword = reader["MemberPassword"].ToString();
                    mem.AccessLevel = Convert.ToByte(reader["MemberAccess"]);
                    mem.TextColor = reader["MemberTextColor"].ToString();
                    mem.DesplayMessage = reader["MemberText"].ToString();
                    mem.UserID = Convert.ToInt32(reader["Member_ID"]);
                    mem.CodeForNoPwLogin = reader["GuidGoesHereJohn"].ToString();
                    return mem;
                }
                else
                    return null;
            }
            finally
            {
                con.Dispose();
            }

        }
    }
}