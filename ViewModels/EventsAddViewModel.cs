using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GBW.ViewModels
{
    public class EventsAddViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}