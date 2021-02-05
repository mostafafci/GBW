using System;
using System.Collections.Generic;
using System.Linq;
using GBW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GBW.Startup))]

namespace GBW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SystemRoles();
            SystemSuperAdmin();
        }


        private void SystemRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole role;
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                role = new IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Vendor"))
            {
                role = new IdentityRole();
                role.Name = "Vendor";
                roleManager.Create(role);
            }

        }
        private void SystemSuperAdmin()
        {
            var user = new ApplicationUser
            {

                UserName = "mostafaharoon1993@gmail.com",
                Email = "mostafaharoon1993@gmail.com",
                PhoneNumber = "01009433965",
                IsActive=true

            };
            ApplicationDbContext dbContext = new ApplicationDbContext();
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(dbContext);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            var result = userManager.Create(user, "P@ssw0rd");
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, "SuperAdmin");

            }
        }
    }
}
