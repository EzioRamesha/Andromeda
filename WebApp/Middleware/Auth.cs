using BusinessObject.Identity;
using Microsoft.AspNet.Identity;
using Services.Identity;
using Shared;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.Middleware
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public string Controller { get; set; }
        public string Power { get; set; }
        public string ReturnController { get; set; }
        public string ReturnAction { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (Controller != null && Power != null)
                {
                    var authUserId = HttpContext.Current.User.Identity.GetUserId<int>();
                    var authUserBo = UserService.Find(authUserId);
                    var valid = false;

                    valid = UserService.CheckPowerFlag(authUserBo, Controller, Power);

                    if (!valid)
                    {
                        // redirect to Dashboard
                        filterContext.HttpContext.Session["ErrorMessage"] = Power == AccessMatrixBo.PowerUpload ? MessageBag.UploadAccessDenied : MessageBag.AccessDenied;
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = ReturnController ?? "Home", Action = ReturnAction ?? "Index" }));
                    }
                }
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}