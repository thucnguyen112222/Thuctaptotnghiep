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
    public class SliderController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Slider
        public ActionResult Index()
        {
            var list = db
                .C_slider.Where(m => m.slider_status != 2)
                .OrderByDescending(m => m.slider_id).ToList();
            return View(list);
        }

        // GET: Admin/Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_slider c_slider = db.C_slider.Find(id);
            if (c_slider == null)
            {
                return HttpNotFound();
            }
            return View(c_slider);
        }

        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.order = db.C_slider.Where(m => m.slider_status != 0).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_slider slider, HttpPostedFileBase file)
        {

            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(slider.slider_name);
                if (db.C_slider.Where(m => m.slider_link == slug).Count() != 0)
                {
                    Thongbao.set_flash("slider đã tồn tại", "danger");
                    return RedirectToAction("Create");
                }
                slider.slider_link = slug;
                slider.slider_createdat = DateTime.Now;
                slider.slider_createdby = 1;
                slider.slider_position = "Slideshow";
                slider.slider_order = slider.slider_order;
                slider.slider_status = slider.slider_status;

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    slider.slider_img = fileName;
                    string path = Path.Combine(Server.MapPath("~/wwwroot/img/"), fileName);
                    file.SaveAs(path);
                }
                else
                {
                    Thongbao.set_flash("Chưa thêm hình ảnh cho slider", "danger");
                    return RedirectToAction("Index");
                }
                db.C_slider.Add(slider);
                db.SaveChanges();
                Thongbao.set_flash("Thêm thành công", "danger");
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.order = db.C_slider.Where(m => m.slider_status != 0).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_slider c_slider = db.C_slider.Find(id);
            if (c_slider == null)
            {
                return HttpNotFound();
            }
            return View(c_slider);
        }

        // POST: Admin/Slider/Edit/5
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(C_slider slider, HttpPostedFileBase file, int id)
        {
            ViewBag.order = db.C_slider.Where(m => m.slider_status != 0).ToList();
            var img = db.C_slider.AsNoTracking()
                   .First(m => m.slider_id == id);
            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(slider.slider_name);
                if (db.C_slider.Where(m => m.slider_link == slug).Count() != 0)
                {
                    Thongbao.set_flash("slider đã tồn tại", "danger");
                    return RedirectToAction("Create");
                }
                var slider_edit = db.C_slider.Find(id);
                slider_edit.slider_name = slider.slider_name;
                slider_edit.slider_link = slug;
                slider_edit.slider_createdat = slider.slider_createdat;
                slider_edit.slider_createdby = slider.slider_createdby;
                slider_edit.slider_position = "SlideShow";
                slider_edit.slider_order = slider.slider_order;
                slider_edit.slider_status = slider.slider_status;
                slider_edit.slider_updatedat = DateTime.Now;
                slider_edit.slider_updatedby = Convert.ToInt32(Session["User_Id_admin"]);
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    slider_edit.slider_img = fileName;
                    string path = Path.Combine(Server.MapPath("~/wwwroot/img/"), fileName);
                    file.SaveAs(path);
                }
                slider_edit.slider_img = img.slider_img;
                slider.slider_img = img.slider_img;
                db.Entry(slider_edit).State = EntityState.Modified;
                db.SaveChanges();
                Thongbao.set_flash("Thay đổi thành công", "danger");
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        public ActionResult Delete(int id)
        {
            C_slider c_slider = db.C_slider.Find(id);
            db.C_slider.Remove(c_slider);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
        public ActionResult DelTrash(int? id)
        {
            C_slider slider = db.C_slider.Find(id);
            if (slider == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            slider.slider_status = 2;
            slider.slider_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }
        public ActionResult ReTrash(int? id)
        {
            C_slider slider = db.C_slider.Find(id);
            if (slider == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            slider.slider_status = 0;
            slider.slider_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã khôi phục", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            var list = db.C_slider.Where(m => m.slider_status == 2);
            return View(list.ToList());
        }
        // GET: Admin/slider/Delete/5

        public ActionResult Status(int id)
        {
            C_slider slider = db.C_slider.Find(id);
            if (slider == null)
            {
                Thongbao.set_flash("Loại sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            slider.slider_status = (slider.slider_status == 1) ? 0 : 1;
            slider.slider_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
