using Microsoft.Owin;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Owin;
using System;
using System.IO;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

   
    }

    
}
