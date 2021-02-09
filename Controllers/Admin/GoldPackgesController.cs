using GBW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace GBW.Controllers.Admin
{
    public class GoldPackgesController : BaseController
    {
        [HttpGet]
        [ResponseType(typeof(List<GoldPackageViewModel>))]
        public List<GoldPackageViewModel> GetAllPackges()
        {
            try
            {
                return UnitOfWork.GoldPackageService.GetAllPackges();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [ResponseType(typeof(GoldPackageViewModel))]
        public GoldPackageViewModel GetGoldPackgesDetails(int id)
        {
            try
            {
                return UnitOfWork.GoldPackageService.GetGoldPackageDetails(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage AddGoldPackage(GoldPackageAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string UserId = GetAuthUserID();
                    var resualt = UnitOfWork.GoldPackageService.AddGoldPackage(model, UserId);
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
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage UpdateGoldPackage(GoldPackageEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //remove Old Links
                    if (model.EducationLinks != null)
                    {
                        var itemDetails =UnitOfWork.GoldPackageService.GetGoldPackageDetails(model.Id);
                        UnitOfWork.GoldPackageEducationLinksService.DeleteEduLinksByPackage(itemDetails.EducationLinks);
                    }

                    var resualt = UnitOfWork.GoldPackageService.UpdateGoldPackage(model);
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
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage DeleteGoldPackage(int id)
        {
            try
            {
                var resualt = UnitOfWork.GoldPackageService.DeleteGoldPackage(id);
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
