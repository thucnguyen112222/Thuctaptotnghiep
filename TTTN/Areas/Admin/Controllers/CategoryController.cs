using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Category
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.C_category.
                Where(m => m.category_status != 2).
                OrderByDescending(m => m.category_createdat).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_category c_category = db.C_category.Find(id);
            if (c_category == null)
            {
                return HttpNotFound();
            }
            return View(c_category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();

            return View();
        }

        // POST: Admin/Category/Create
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_category category)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();

            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(category.category_name);
                category.category_slug = slug;
                category.category_createdat = DateTime.Now;
                category.category_createby = 1;
                C_link link = new C_link();
                link.slug = slug;
                link.tableid = category.category_id;
                link.type = "category";
                db.C_link.Add(link);
                db.C_category.Add(category);
                db.SaveChanges();
                Thongbao.set_flash("Thêm thành công", "success");
                return RedirectToAction("Index");
            }
            Thongbao.set_flash("Thêm thất bại", "success");
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_category c_category = db.C_category.Find(id);
            if (c_category == null)
            {
                return HttpNotFound();
            }
            return View(c_category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(C_category category, int id)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();
            if (!ModelState.IsValid)
            {
                string url = Xstring.ToAscii(category.category_name);
                var list = db.C_category
                 .Where(m => m.category_slug == url && m.category_id == id)
                 .ToList();
                if (list.Count() == 1)
                {
                    Thongbao.set_flash("Loại sản phẩm đã tồn tại", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    var Cat = db.C_category.Find(id);
                    if (Cat == null)
                        return RedirectToAction("Index");
                    Cat.category_slug = url;
                    Cat.category_createdat = DateTime.Now;
                    Cat.category_createby = 1;
                    Cat.category_name = category.category_name;
                    Cat.category_updatedat = DateTime.Now;
                    Cat.category_updatedby = 1;
                    db.Entry(Cat).State = EntityState.Modified;
                    var item_link = db.C_link
                        .Where(m => m.type == "category" && m.tableid == Cat.category_id).First();
                    item_link.slug = url;
                    db.Entry(item_link).State = EntityState.Modified;
                    int sl_row_cap_nhat = db.SaveChanges();
                    Thongbao.set_flash("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5

        public ActionResult Delete(int id)
        {
            C_category c_category = db.C_category.Find(id);
            foreach (var item in db.C_product.Where(m => m.product_catid == c_category.category_id))
            {
                db.C_product.Remove(item);
            }
            db.C_category.Remove(c_category);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            C_category category = db.C_category.Find(id);
            if (category == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "success");
                return RedirectToAction("Index");
            }
            category.category_status = (category.category_status == 1) ? 2 : 1;
            category.category_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            var list = db.C_category.Where(m => m.category_status == 2);
            return View(list.ToList());
        }
        // GET: Admin/Product/Delete/5

        public ActionResult Status(int id)
        {
            C_category category = db.C_category.Find(id);
            if (category == null)
            {
                Thongbao.set_flash("Loại sản phẩm không tồn tại", "success");
                return RedirectToAction("Index");
            }
            category.category_status = (category.category_status == 1) ? 0 : 1;
            category.category_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
