
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TTTN
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            Session["SL_Cart"] = null;
            Session["Cart"] = null;
            Session["Thong_bao"] = null;
            Session["User"] = null;
            Session["User_Id"] = null;
            Session["User_Admin"] = null;
            Session["User_Id_admin"] = null;
            Session["User_Img_admin"] = null;
            Session["User_Acess_admin"] = null;
            
        }
    }
}
