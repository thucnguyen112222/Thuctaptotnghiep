using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var list = db.C_product
                 .Where(m => m.product_status != 2)
                 .OrderByDescending(m => m.product_id);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_product c_product = await db.C_product.FindAsync(id);
            if (c_product == null)
            {
                return HttpNotFound();
            }

            var cat = db.C_category
                .Where(m => m.category_id == c_product.product_catid).First();
            ViewBag.catname = cat.category_name;
            return View(c_product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.list = db.C_category.Where(m => m.category_status == 1).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(C_product product, HttpPostedFileBase file)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status == 1)
                                        .ToList();
            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(product.product_name);
               
                    if (db.C_product.Where(m => m.product_slug == slug).Count() != 0)
                    {
                        Thongbao.set_flash(" sản phẩm đã tồn tại", "danger");
                        return RedirectToAction("Create");
                    }
                    product.product_name = product.product_name;
                    product.product_slug = slug;
                    product.product_createdat = DateTime.Now;
                    product.product_createdby = Convert.ToInt32(Session["User_Id_admin"]);
                    product.product_detail = product.product_detail;
                    product.product_catid = product.product_catid;
                    product.product_pricesale = (product.product_pricesale != 0 ? product.product_pricesale : 0);
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);

                        if (fileName.Length < 100)
                        {
                            product.product_img = fileName;
                            string path = Path.Combine(Server.MapPath("~/wwwroot/img/"), fileName);
                            file.SaveAs(path);
                            db.C_product.Add(product);
                            db.SaveChanges();
                        }
                        else
                        {
                            Thongbao.set_flash("Tên hình ảnh không vượt quá 100 kí tự", "danger");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        Thongbao.set_flash("Thêm thất bại", "danger");
                        return RedirectToAction("Index");
                    }
                
                Thongbao.set_flash("Thêm thành công", "danger");
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var c_product = db.C_product
                .Where(m => m.product_id == id && m.product_status != 2).First();
            if (c_product == null)
            {
                return HttpNotFound();
            }
            return View(c_product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(C_product product, int id, HttpPostedFileBase file)
        {
            ViewBag.list = db.C_category.Where(m => m.category_status != 0).ToList();
            if (!ModelState.IsValid)
            {
                var img = db.C_product.AsNoTracking()
                    .First(m => m.product_id == id);
                string slug = Xstring.ToAscii(product.product_name);
                var list = db.C_product
                    .Where(m => m.product_slug == slug && m.product_id != id)
                    .ToList();
                if (list.Count() != 0)
                {
                    Thongbao.set_flash("sản phẩm đã tồn tại", "danger");
                    return RedirectToAction("Index");
                }
                product.product_id = id;
                product.product_slug = slug;
                product.product_updatedat = DateTime.Now;
                product.product_updatedby = int.Parse(Session["User_Id_admin"].ToString());
                if (file != null && file.ContentLength > 0)
                {
                    string directoryimg = Server.MapPath("~/wwwroot/img/" + img.product_img);
                    System.IO.File.Delete(directoryimg);
                    string fileName = Path.GetFileName(file.FileName);
                    product.product_img = fileName;
                    string path = Path.Combine(Server.MapPath("~/wwwroot/img/"), fileName);
                    file.SaveAs(path);
                }
                else
                {
                    product.product_img = img.product_img;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                Thongbao.set_flash("Cập nhật thành công!", "danger");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult DelTrash(int? id)
        {
            C_product product = db.C_product.Find(id);
            if (product == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            product.product_status = 2;
            product.product_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }
        public ActionResult ReTrash(int? id)
        {
            C_product product = db.C_product.Find(id);
            if (product == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            product.product_status = 0;
            product.product_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã khôi phục", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            var list = db.C_product.Where(m => m.product_status == 2);
            return View(list.ToList());
        }
        // GET: Admin/Product/Delete/5

        public ActionResult Delete(int id)
        {
            C_product c_product = db.C_product.Find(id);
            db.C_product.Remove(c_product);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Status(int id)
        {
            C_product product = db.C_product.Find(id);
            if (product == null)
            {
                Thongbao.set_flash("Loại sản phẩm không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            product.product_status = (product.product_status == 1) ? 0 : 1;
            product.product_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
