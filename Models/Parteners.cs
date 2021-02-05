using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GBW.Models
{
    public class Parteners
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string WebsiteURL { get; set; }

        public string AddedBy { get; set; }
        [ForeignKey("AddedBy")]
        public ApplicationUser User { get; set; }
        public DateTime AddedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}