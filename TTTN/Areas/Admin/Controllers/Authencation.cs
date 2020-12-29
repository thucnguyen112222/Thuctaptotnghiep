using System.Linq;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class AuthencationController : Controller
    {
        TTTNdbContext db = new TTTNdbContext();
        //login ADMIN
        public ActionResult Login()
        {
            if (Session["User_Admin"] != null)
            {
                Response.Redirect("~/Admin");
            }
            return View();
        }
        [HttpPost]

        public ActionResult Login(FormCollection fied)
        {
            string username = fied["username"];
            string password = Xstring.ToMD5(fied["password"]);
            var count_username = db.C_user
                .Where(m => m.user_username == username && m.user_access != 0 && m.user_status == 1);
            if (count_username.Count() == 0)
            {
                ViewBag.Error = "<span class ='text-danger'>Tài khoản không tồn tại!</span>";
            }
            else
            {
                var count_password = db.C_user
                .Where(m => m.user_username == username && m.user_access != 0 && m.user_status == 1 && m.user_password == password);
                if (count_password.Count() == 0)
                {
                    ViewBag.Error = "<span class ='text-danger'>Mật khẩu không chính xác!</span>";
                }
                else
                {
                    var use = count_password.First();
                    Session["User_Admin"] = use.user_fullname;
                    Session["User_Id_admin"] = use.user_id;
                    Session["User_Acess_admin"] = use.user_access;
                    Session["User_Img_admin"] = use.user_img;
                    Response.Redirect("~/Admin");
                }
            }
            return View("Login");
        }
        //Logout ADMIN
        public void Logout()
        {
            if (Session["User_Admin"] != null)
            {
                Session.Remove("User_Admin");
                Session.Remove("User_Id");
                Response.Redirect("~/Admin");
            }
            else
            {
                Response.Redirect("~/Admin/login");
            }
        }
    }
}