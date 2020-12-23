using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Users
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["User_Id_admin"].ToString());
            var list_user = db.C_user.Where(m => m.user_status != 2 && m.user_id != id).ToList();
            return View(list_user);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_user c_user = db.C_user.Find(id);
            if (c_user == null)
            {
                return HttpNotFound();
            }
            return View(c_user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_user user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (db.C_user.Where(m => m.user_username == user.user_username).Count() == 0)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string pass = Xstring.ToMD5(user.user_password);
                        user.user_password = pass;
                        string fileName = Path.GetFileName(file.FileName);
                        user.user_createdat = DateTime.Now;
                        user.user_createdby = Convert.ToInt32(Session["User_Id_Admin"].ToString());
                        user.user_img = fileName;
                        string path = Path.Combine(Server.MapPath("~/wwwroot/img/"), fileName);
                        file.SaveAs(path);
                        db.C_user.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Thongbao.set_flash("Thêm thất bại", "danger");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Thongbao.set_flash("UserName đã tồn tại", "danger");
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_user c_user = db.C_user.Find(id);
            if (c_user == null)
            {
                return HttpNotFound();
            }
            return View(c_user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_fullname,user_username,user_password,user_email,user_gender,user_phone,user_img,user_access,user_createdat,user_createdby,user_updatedat,user_updatedby,user_status,user_address")] C_user c_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c_user);
        }

        public ActionResult Delete(int id)
        {
            C_user c_user = db.C_user.Find(id);
            db.C_user.Remove(c_user);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "succsess");
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            C_user user = db.C_user.Find(id);
            if (user == null)
            {
                Thongbao.set_flash("User không tồn tại", "succsess");
                return RedirectToAction("Index");
            }
            user.user_status = 2;
            user.user_updatedat = DateTime.Now;
            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }
        public ActionResult Undo(int Id)
        {
            C_user user = db.C_user.Find(Id);
            if (user == null)
            {
                Thongbao.set_flash("User không tồn tại", "succsess");
                return RedirectToAction("Index");
            }
            user.user_status = 0;
            user.user_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công", "success");
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            var list = db.C_user.Where(m => m.user_status == 2);
            return View(list.ToList());
        }
        // GET: Admin/Product/Delete/5


        public ActionResult Status(int Id)
        {
            C_user user = db.C_user.Find(Id);
            if (user == null)
            {
                Thongbao.set_flash("User không tồn tại", "succsess");
                return RedirectToAction("Index");
            }
            user.user_status = (user.user_status == 1) ? 0 : 1;
            user.user_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
