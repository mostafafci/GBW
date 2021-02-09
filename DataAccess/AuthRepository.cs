using GBW.Models;
using GBW.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace GBW.DataAccess
{
    public class AuthRepository : IDisposable
    {
        private ApplicationDbContext _ctx;

        private UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _ctx = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            string ReferralLink = CreateReferralLink(userModel.Email);
            string InvitedUserId = GetUserIdFromReferralLink(userModel.InvitedReferralLink);
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                PhoneNumber = userModel.Phone,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                ReferralLink = ReferralLink,
                InvitedUserId= InvitedUserId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
                userManager.AddToRole(user.Id, "User");
                _ctx.SaveChanges();
            }
            return result;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                user = await _userManager.FindAsync(user.UserName, password);
            }
            return user;
        }
        public async Task<IdentityResult> ChangePassword(ChangePasswordBindingModell model)
        {

            IdentityResult result = await _userManager.ChangePasswordAsync(model.AspNetUserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
            }
            return result;
        }

        public async Task<int> ForgotPassword(ForgotPasswordViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string password = Membership.GeneratePassword(12, 1);
                var result = await _userManager.RemovePasswordAsync(user.Id);
                if (result.Succeeded)
                {
                    var resultPass = await _userManager.AddPasswordAsync(user.Id, password);
                    if (resultPass.Succeeded)
                    {
                        WebClient client = new WebClient();
                        string GetTempDomain = WebConfigurationManager.AppSettings["PortalDomain"];
                        String htmlCode = client.DownloadString(GetTempDomain + "/EmailTemplete/ResetPassword.html");
                        IdentityMessage item = new IdentityMessage();
                        item.Body = htmlCode;
                        item.Body = HttpUtility.HtmlDecode(item.Body);
                        item.Destination = model.Email;
                        item.Subject = "";
                        EmailService.SendConfirmationLink(item);
                        return 200;
                    }
                    else
                    {
                        return 500;
                    }

                }
                else
                {
                    return 500;
                }
            }
            else { return 404; }

        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }
        public ApplicationUser FindByEmail(string Email)
        {
            ApplicationUser user = _userManager.FindByEmail(Email);

            return user;
        }
        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }


        private string CreateReferralLink(string Email)
        {
            List<string> Lst = Email.Split('@').ToList();
            if (Lst.Count>0)
            {
                //string Domain = WebConfigurationManager.AppSettings["ApplicationFrontDomain"];
                return Lst.FirstOrDefault();
            }
            return null;
        }

        private string GetUserIdFromReferralLink(string ReferralLink)
        {
            string Domain = WebConfigurationManager.AppSettings["ApplicationFrontDomain"];
            if (ReferralLink.Contains(Domain))
            {
                ReferralLink = ReferralLink.Replace(Domain, "");
            }
            var user = _ctx.Users.Where(x => x.ReferralLink == ReferralLink && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return user.Id;
        }
    }
    public class ChangePasswordBindingModell
    {
        [Required]
        [Display(Name = "AspNetUserId")]
        public string AspNetUserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public static class EmailService
    {
        public static void SendConfirmationLink(IdentityMessage message)
        {
            //Mostafa Nassar
            // Plug in your email service here to send an email.
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailPassword"]);
            var msg = new MailMessage(ConfigurationManager.AppSettings["mailAccount"], message.Destination, message.Subject, message.Body);
            msg.IsBodyHtml = true;
            //client.Send(ConfigurationManager.AppSettings["mailAccount"], message.Destination, message.Subject, message.Body);
            client.Send(msg);

        }
    }

}