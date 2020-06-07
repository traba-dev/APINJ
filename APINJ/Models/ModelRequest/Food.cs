using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APINJ.Models.ModelRequest
{
    public class Food
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string img { get; set; }
        public string status { get; set; }

        public int category { get; set; }
    }
}