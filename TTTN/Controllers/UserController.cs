using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Controllers
{
    public class UserController : DefaultController
    {
        TTTNdbContext db = new TTTNdbContext();
        // GET: User
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                int ID = Convert.ToInt32(Session["User_Id"].ToString());
                ViewBag.user_Login = db.C_user.Find(ID);

                var listt = db.C_order
                    .Where(m => m.order_userid == ID && m.order_status != 1)
                    .ToList();
                
                var list_product = db.C_orderdetail.ToList();

                ViewBag.Donhang = listt;
                ViewBag.list_productss = list_product;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return View();
            }
            return RedirectToAction("Home", "Home");
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            ViewBag.flag = false;
            string username = field["user_email"];
            string password = Xstring.ToMD5(field["user_password"]);

            var user = db.C_user
                .Where(m => m.user_username == username || m.user_email == username && m.user_status == 1 && m.user_access == 0);


            if (user.Count() == 0)
            {
                Thongbao.set_flash("Tài khoản không tồn tại", "danger");
                return View();
            }
            else
            {
                var pass = db.C_user
                    .Where(m => m.user_password == password && m.user_username == username || m.user_email == username && m.user_status == 1 && m.user_access == 0);
                if (pass.Count() == 0)
                {
                    Thongbao.set_flash("Mật khẩu không đúng", "danger");
                    return View();
                }
                else
                {
                    ViewBag.flag = true;
                    var use = pass.First();
                    Session["User"] = use.user_username;
                    Session["User_Id"] = use.user_id;
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            string username = form["username"];
            string password = Xstring.ToMD5(form["password"]);
            string xacnhanpw = Xstring.ToMD5(form["xacnhanpw"]);
            string fullname = form["fullname"];

            string email = form["email"];
            string phone = form["phone"];
            string address = form["address"];
            if (ModelState.IsValid)
            {
                var checkmail = db.C_user.Where(m => m.user_email == email);
                var checkusername = db.C_user.Where(m => m.user_username == username);

                if (checkmail.Count() > 0)
                {
                    Thongbao.set_flash("Địa chỉ email đã tồn tại", "danger");
                    return RedirectToAction("Create");
                }
                else if (checkusername.Count() > 0)
                {
                    Thongbao.set_flash("Tên đăng nhập đã tồn tại", "danger");
                    return RedirectToAction("Login");
                }
                else
                {
                    if (password != xacnhanpw)
                    {
                        Thongbao.set_flash("Xác nhận mật khẩu không chính xác", "danger");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        var user = new C_user();
                        user.user_username = username;
                        user.user_password = password;
                        user.user_fullname = fullname;
                        user.user_email = email;
                        user.user_phone = phone;
                        user.user_address = address;
                        user.user_img = null;
                        user.user_access = 0;
                        user.user_status = 1;
                        user.user_createdat = DateTime.Now;
                        user.user_updatedat = DateTime.Now;

                        db.C_user.Add(user);
                        db.SaveChanges();
                        int count = db.C_user.Where(m => m.user_username == username).Count();
                        if (count > 0)
                        {
                            Thongbao.set_flash("Đăng kí thành công", "success");
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            Thongbao.set_flash("Đăng kí không thành công", "success");
                            return RedirectToAction("Login");
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult UpdateUser(int? id)
        {
            if (Session["User"] != null)
            {
                C_user Item_user = db.C_user.Find(id);
                if (Item_user == null)
                {
                    Thongbao.set_flash("User không tồn tại", "danger");
                    return RedirectToAction("Index");
                }
                return View(Item_user);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        public ActionResult UpdateUser(FormCollection form, int? id, HttpPostedFileBase file)
        {
            string user_fullname = form["user_fullname"];
            string user_gender = form["user_gender"];
            string user_email = form["user_email"];
            string user_phone = form["user_phone"];
            string user_address = form["user_address"];
            if (Session["User"] != null)
            {
                var item = db.C_user.Find(id);
                item.user_email = user_email;
                item.user_address = user_address;
                item.user_fullname = user_fullname;
                item.user_gender = Convert.ToInt32(user_gender);
                item.user_phone = user_phone;
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    item.user_img = fileName;
                    string path = Path.Combine(Server.MapPath("~/wwwroot/img/user"), fileName);
                    file.SaveAs(path);
                }
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public void Logout()
        {
            Session["User"] = null;
            Session["User_Id"] = null;
            Response.Redirect("~/");
        }
    }
}