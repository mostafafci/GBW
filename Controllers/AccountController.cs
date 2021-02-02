﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using GBW.Models;
using GBW.Providers;
using GBW.Results;
using System.Web.Http.Description;
using GBW.DataAccess;
using GBW.ViewModels;
using System.Net;
using System.Linq;
using Newtonsoft.Json;

namespace GBW.Controllers
{
    public class AccountController : BaseController
    {
        private AuthRepository _repo = null;
        public AccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [ResponseType(typeof(HttpResponseMessage))]
        public async Task<HttpResponseMessage> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            if (_repo.FindByEmail(userModel.Email)!=null)
            {
                //ModelState.AddModelError("Invalid Email","This Email Is Exist In Database");
                //return BadRequest(ModelState);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This Email Is Exist In Database");
            }
            IdentityResult result = await _repo.RegisterUser(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, result.Errors.FirstOrDefault());
            }
            ApplicationUser user = await _repo.FindUser(userModel.Email, userModel.Password);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            string Token = GetToken(userModel.Email, userModel.Password);
            var obj = JsonConvert.DeserializeObject<TokenData>(Token);
            LoginData loginData = new LoginData();
            loginData.TokenData = obj;
            loginData.UserID = user.Id;
            loginData.UserName = user.UserName;
            //return Ok(Token);
            return Request.CreateResponse<LoginData>(HttpStatusCode.OK, loginData);
        }

        // POST api/Account/LogIn
        [AllowAnonymous]
        [ResponseType(typeof(IHttpActionResult))]
        public async Task<IHttpActionResult> LogIn(UserLoginViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await _repo.FindUser(userModel.UserName, userModel.Password);
            if (user == null)
            {
                return NotFound();
            }
            string Token = GetToken(userModel.UserName, userModel.Password);
            var obj = JsonConvert.DeserializeObject<TokenData>(Token);
            LoginData loginData = new LoginData();
            loginData.TokenData = obj;
            loginData.UserID = user.Id;
            loginData.UserName = user.UserName;
            return Ok(loginData);
        }

        // POST api/Account/ChangePassword
        [Authorize]
        [ResponseType(typeof(HttpResponseMessage))]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordBindingModell model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            IdentityResult result = await _repo.ChangePassword(model);
            if (!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

            return Request.CreateResponse<string>(HttpStatusCode.OK, "Password Changed Successfully The New Password is : " + model.NewPassword);
        }
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(HttpResponseMessage))]
        public async Task<HttpResponseMessage> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resualt = await _repo.ForgotPassword(model);
                if (resualt == 200)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.OK, "Your Password Changed Successfuly Blease Check Your Email");
                }
                else if (resualt == 404)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Email Not Found");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error");
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string GetToken(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", userName ),
                        new KeyValuePair<string, string> ( "Password", password )
                    };
            var content = new FormUrlEncodedContent(pairs);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                string host = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                              ":" + HttpContext.Current.Request.Url.Port + "/";
                var response = client.PostAsync(host + "token", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        private string GetAuthUserID()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userWithClaims = (ClaimsPrincipal)User;
                //var email = userWithClaims.FindFirst(ClaimTypes.Email).Value;
                string UserEmail = userWithClaims.Claims.FirstOrDefault().Value;
                var user = _repo.FindByEmail(UserEmail);
                if (user != null)
                {
                    return user.Id;
                }
                return null;
            }
            return null;
        }
    }

    internal class LoginData
    {
        public TokenData TokenData { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
    }
    public class TokenData
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
