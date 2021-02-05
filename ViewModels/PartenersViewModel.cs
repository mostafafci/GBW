using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GBW.ViewModels
{
    public class PartenersViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public string WebsiteURL { get; set; }
    }
}