using System.Web.Mvc;
using System.Web.Routing;

namespace TTTN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Profile",
               url: "user/profile",
               defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Update",
                url: "user/update",
                defaults: new { controller = "User", action = "UpdateUser", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "user/dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Logout",
                url: "user/dang-xuat",
                defaults: new { controller = "User", action = "Logout", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Contact",
               url: "lien-he",
               defaults: new { controller = "Contacts", action = "Create", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "news",
                url: "tin-tuc",
                defaults: new { controller = "Home", action = "Topic", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Payment",
               url: "thanh-toan",
               defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "productAll",
               url: "san-pham",
               defaults: new { controller = "Home", action = "ProductAll", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Sale",
              url: "san-pham-khuyen-mai",
              defaults: new { controller = "Home", action = "ProductSaleAll", id = UrlParameter.Optional }
          );


            routes.MapRoute(
            name: "Catagory",
            url: "{slug}",
            defaults: new { controller = "Home", action = "ProductCategory", id = UrlParameter.Optional }
        );

            routes.MapRoute(
            name: "SiteURL",
            url: "{slug}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
        }
    }
}
