using BusinessObject.Identity;
//using DataAccess.Migrations;
using Microsoft.Owin.Security;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System;
//using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //(new DbMigrator(new Configuration())).Update();
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            var statusCode = Response.StatusCode;
            if (ex is HttpException httpEx)
            {
                statusCode = httpEx.GetHttpCode();
            }

            bool logError = true;
            if (ex is HttpAntiForgeryException)
            {

                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                UrlHelper urlHelper = new UrlHelper(Request.RequestContext);
                RouteData routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
                if (routeData != null)
                {
                    string action = routeData.Values["action"] as string;
                    string controller = routeData.Values["controller"] as string;

                    if (controller == "Account" && action == "Login" && Context.User != null && Context.User.Identity.IsAuthenticated)
                    {
                        logError = false;
                        Response.Redirect("/Home/Index");
                    }
                }

            }

            if (statusCode != 404 && logError)
            {
                string path = string.Format("{0}/WebAppErrorLog".AppendDateFileName(".txt"), Util.GetLogPath("WebApp"));
                Util.MakeDir(path);
                using (var textFile = new TextFile(path, true, true))
                {
                    textFile.WriteLine(string.Format("{0}   {1}", DateTime.Now.ToString(Util.GetDateTimeConsoleFormat()), ex));
                    textFile.WriteLine();
                    textFile.WriteLine();
                }
            }
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0;
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                bool allowMultipleLogin = bool.Parse(Util.GetConfig("AllowMultipleLogin"));
                string userName = HttpContext.Current.User.Identity.Name;
                string sessionId = HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : "";

                if (!string.IsNullOrEmpty(sessionId))
                {
                    UserBo userBo = UserService.FindByUsername(userName);
                    if (userBo == null || userBo.Status == UserBo.StatusSuspend || (userBo.SessionId != sessionId && !allowMultipleLogin))
                    {
                        IAuthenticationManager AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                        if (AuthenticationManager != null)
                            AuthenticationManager.SignOut();
                        Response.Redirect("/Account/Login");
                    } 
                    else if (HttpContext.Current.Request != null)
                    {
                        HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                        UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                        RouteData routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
                        if (routeData != null)
                        {
                            string action = routeData.Values["action"] as string;

                            if (action != "CheckUserSession")
                                Session["lastActivity"] = DateTime.Now.Ticks;
                        }
                    }
                }
            }
        }
    }
}
