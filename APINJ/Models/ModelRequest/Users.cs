using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APINJ.Models.ModelRequest
{
    public class Users
    {
        public int U_Id { get; set; }
        public string U_Name { get; set; }
        public string U_Apellidos { get; set; }
        public string U_Email { get; set; }
        public string U_Nick { get; set; }
        public string U_Pass { get; set; }
        public string U_Status { get; set; }
        public string U_Gender { get; set; }
    }
}