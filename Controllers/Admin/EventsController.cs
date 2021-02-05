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
    public class EventsController : BaseController
    {
        [HttpGet]
        [ResponseType(typeof(List<EventsViewModel>))]
        public List<EventsViewModel> GetAllEvents()
        {
            try
            {
                return UnitOfWork.EventsService.GetAllEvents();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [ResponseType(typeof(EventsViewModel))]
        public EventsViewModel GetEventDetails(int id)
        {
            try
            {
                return UnitOfWork.EventsService.GetEventDetails(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage AddEvent(EventsAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string UserId = GetAuthUserID();
                    var resualt = UnitOfWork.EventsService.AddEvent(model, UserId);
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
        public HttpResponseMessage UpdateEvent(EventsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var resualt = UnitOfWork.EventsService.UpdateEvent(model);
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
        public HttpResponseMessage DeleteEvent(int id)
        {
            try
            {
                var resualt = UnitOfWork.EventsService.DeleteEvent(id);
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
