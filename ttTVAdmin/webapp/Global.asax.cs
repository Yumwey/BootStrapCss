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
            ModelBinders.Binders.Add(typeof(DateTime), new ttTVMS.Models.ttTVDateTimeModelBinder());
            
            System.Data.Entity.Database.SetInitializer<ttTVMS.Models.ServiceDeskContext>(null);
            System.Data.Entity.Database.SetInitializer<ttTVMS.Models.ttTVContext>(null);
            System.Data.Entity.Database.SetInitializer<ttTVMS.Models.ttTVLogContext>(null);

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