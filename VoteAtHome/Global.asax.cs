using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VoteAtHome.Models;

namespace VoteAtHome
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            VoteDB.CreateDatabase();
            VoteDB.CreateTables();
            //VoteDB.FillingVote("Хура",99,"Голосуй за меня если хочешь блокировки","Полит-шиз",1,"Hura.jpg");
        }
    }
}
