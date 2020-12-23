using System.Web.Mvc;

namespace TTTN.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        public BaseController()
        {
            if (System.Web.HttpContext.Current.Session["User_Admin"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/admin/login");
            }

        }
    }
}