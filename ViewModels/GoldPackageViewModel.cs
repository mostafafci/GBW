using GBW.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GBW.ViewModels
{
    public class GoldPackageViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public double NumberOfGram { get; set; }
        public double Value { get; set; }
        public double RevenuePerMonth { get; set; }
        public int NumberOfCards { get; set; }

        public List<GoldPackageEducationLinks> EducationLinks { get; set; }
    }

    public class GoldPackageAddViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public double NumberOfGram { get; set; }
        public double Value { get; set; }
        public double RevenuePerMonth { get; set; }
        public int NumberOfCards { get; set; }

        public List<GoldPackageEducationLinksViewModel> EducationLinks { get; set; }
    }

    public class GoldPackageEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public double NumberOfGram { get; set; }
        public double Value { get; set; }
        public double RevenuePerMonth { get; set; }
        public int NumberOfCards { get; set; }

        public List<GoldPackageEducationLinksViewModel> EducationLinks { get; set; }
    }

    public class GoldPackageEducationLinksViewModel
    {
        public string Name { get; set; }
        public string EduLink { get; set; }
    }
}