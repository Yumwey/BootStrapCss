#region Using

using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace ttTVAdmin
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new Models.ttTVDateTimeModelBinder());
            
            System.Data.Entity.Database.SetInitializer<Models.ServiceDeskContext>(null);
            System.Data.Entity.Database.SetInitializer<Models.ttTVContext>(null);
            System.Data.Entity.Database.SetInitializer<Models.ttTVLogContext>(null);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AuthConfig.RegisterAuth();
        }
    }
}