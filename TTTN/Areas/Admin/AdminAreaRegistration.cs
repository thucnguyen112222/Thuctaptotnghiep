using System.Web.Mvc;

namespace TTTN.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "AdminLogout",
               "Admin/logout",
               new { Controller = "Authencation", action = "Logout", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "AdminLogin",
               "Admin/login",
               new { Controller = "Authencation", action = "Login", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}