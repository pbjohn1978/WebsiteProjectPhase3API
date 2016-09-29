using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteProjectPhase1.Models
{
    public class SiteMember
    {   
        [Required]
        //TODO: add in a methoud in the HelperDB class that will help this tell if the username is already in the db...
        // TODO: add in Compare attribute to Username
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Confirm Password entered does not match Password entered.")]
        public string ConfirmPassword { get; set; }

        // TODO: add in to the HelperDB class a methoud that takes a string and returns a string
        // TODO: add in Compare attribute to BackgroundColor
        public string BackgroundColor { get; set; }

        // TODO: add in to the HelperDB class a methoud that takes a string and returns a string
        // TODO: add in Compare attribute to TextColor
        public string TextColor { get; set; }

        public string DesplayMessage { get; set; }

        public byte AccessLevel { get; set; }

        public int UserID { get; set; }

        public string CodeForNoPwLogin { get; set; }
    }
}