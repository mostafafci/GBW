using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GBW.Models
{
    public class GoldPackageEducationLinks
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EduLink { get; set; }

        //public string GoldPackage_Id { get; set; }
    }
}