using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.ViewModels
{
    public class UsersDataViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string ReferralLink { get; set; }
        public bool Statues { get; set; }
    }
}