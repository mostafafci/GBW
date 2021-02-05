using GBW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace GBW.Controllers.Admin
{
    public class PartenersController : BaseController
    {
        // GET: Parteners
        [HttpGet]
        [ResponseType(typeof(List<PartenersViewModel>))]
        public List<PartenersViewModel> GetAllPartners()
        {
            try
            {
                return UnitOfWork.PartenersService.GetAllPartners();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [ResponseType(typeof(PartenersViewModel))]
        public PartenersViewModel GetPartnerDetails(int id)
        {
            try
            {
                return UnitOfWork.PartenersService.GetPartnerDetails(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage AddPartner(PartenersAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string UserId = GetAuthUserID();
                    var resualt = UnitOfWork.PartenersService.AddPartner(model, UserId);
                    if (resualt == true)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "Done");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not Found");
                    }
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex.ToString());
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage UpdatePartner(PartenersViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var resualt = UnitOfWork.PartenersService.UpdatePartner(model);
                    if (resualt == true)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "Done");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not Found");
                    }
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex.ToString());
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage DeletePartner(int id)
        {
            try
            {
                var resualt = UnitOfWork.PartenersService.DeletePartner(id);
                if (resualt == true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "Done");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        private string GetAuthUserID()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userWithClaims = (ClaimsPrincipal)User;
                //var email = userWithClaims.FindFirst(ClaimTypes.Email).Value;
                string UserId = userWithClaims.Claims.FirstOrDefault().Value;
                //var user = _repo.FindByEmail(UserEmail);
                if (UserId != null)
                {
                    return UserId;
                }
                return null;
            }
            return null;
        }
    }
}