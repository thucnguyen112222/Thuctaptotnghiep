using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/C_menu
        public ActionResult Index()
        {
            ViewBag.listCat = db.C_category.Where(m => m.category_status == 1).ToList();
            var menu_list = db.C_menu
                .Where(m => m.menu_status != 2 && m.menu_position == "Menu").ToList();
            return View(menu_list);
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                var itemCat = form["nameCategory"];
                if (!string.IsNullOrEmpty(itemCat))
                {
                    var arrCat = itemCat.Split(',');
                    int dem = 0;
                    foreach (var item in arrCat)
                    {
                        int id = int.Parse(item);
                        C_category category = db.C_category.Find(id);
                        C_menu menu = new C_menu();
                        menu.menu_name = category.category_name;
                        menu.menu_link = category.category_slug;
                        menu.menu_type = "Category";
                        menu.menu_tableid = id;
                        menu.menu_order = 1;
                        menu.menu_position = "menu";
                        menu.menu_parentid = 0;
                        menu.menu_status = 0;
                        menu.menu_createdat = DateTime.Now;
                        menu.menu_createdby = Convert.ToInt32(Session["User_Id_admin"]);
                        menu.menu_updatedat = DateTime.Now;
                        menu.menu_updatedby = Convert.ToInt32(Session["User_Id_admin"]);
                        db.C_menu.Add(menu);
                        db.SaveChanges();
                        dem++;
                    }
                    Thongbao.set_flash("Đã thêm thành công " + dem + " menu", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    Thongbao.set_flash("Chưa chọn loại sản phẩm", "success");
                    return RedirectToAction("Index");
                }

            }

            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                var itemCat = form["ThemCustom"];
                if (!string.IsNullOrEmpty(itemCat))
                {
                    C_menu menu = new C_menu();
                    menu.menu_name = form["name"];
                    menu.menu_link = form["link"];
                    menu.menu_type = "custom";
                    menu.menu_tableid = 1;
                    menu.menu_order = 1;
                    menu.menu_position = "menu";
                    menu.menu_parentid = 0;
                    menu.menu_status = 0;
                    menu.menu_createdat = DateTime.Now;
                    menu.menu_createdby = Convert.ToInt32(Session["User_Id_admin"]);
                    menu.menu_updatedat = DateTime.Now;
                    menu.menu_updatedby = Convert.ToInt32(Session["User_Id_admin"]);
                    db.C_menu.Add(menu);
                    db.SaveChanges();

                    Thongbao.set_flash("Đã thêm thành công ", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    Thongbao.set_flash("Thêm thất bại", "success");
                    return RedirectToAction("Index");
                }
            }
            ViewBag.listCat = db.C_category.Where(m => m.category_status == 1).ToList();
            var menu_list = db.C_menu
                .Where(m => m.menu_status != 2 && m.menu_position == "Menu").ToList();
            return View(menu_list);
        }

        // GET: Admin/C_menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_menu c_menu = db.C_menu.Find(id);
            if (c_menu == null)
            {
                return HttpNotFound();
            }
            return View(c_menu);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Menu_parentid = db
                .C_menu.Where(m => m.menu_status != 2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_menu c_menu = db.C_menu.Find(id);
            if (c_menu == null)
            {
                return HttpNotFound();
            }
            return View(c_menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(C_menu menu, int id)
        {
            ViewBag.Menu_parentid = db
                   .C_menu.Where(m => m.menu_status == 1 && m.menu_position == "menu");
            string url = Xstring.ToAscii(menu.menu_name);
            var list = db.C_menu.Where(m => m.menu_id == 1 && m.menu_link == url).ToList();
            if (!ModelState.IsValid)
            {
                if (list.Count > 0)
                {
                    Thongbao.set_flash("Menu đã tồn tại", "danger");
                    return RedirectToAction("Index");
                }
                var Menu_edit = db.C_menu.Find(id);
                Menu_edit.menu_name = menu.menu_name;
                Menu_edit.menu_link = menu.menu_link;
                Menu_edit.menu_position = "menu";
                Menu_edit.menu_parentid = menu.menu_parentid;
                Menu_edit.menu_type = "menu";
                Menu_edit.menu_updatedat = DateTime.Now;
                Menu_edit.menu_updatedby = Convert.ToInt32(Session["User_Id_admin"]);
                db.Entry(Menu_edit).State = EntityState.Modified;
                int sl_row_cap_nhat = db.SaveChanges();
                Thongbao.set_flash("Cập nhật thành công", "success");
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/C_menu/Delete/5

        public ActionResult Delete(int id)
        {
            C_menu c_menu = db.C_menu.Find(id);
            db.C_menu.Remove(c_menu);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "succsess");
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            C_menu menu = db.C_menu.Find(id);
            if (menu == null)
            {
                Thongbao.set_flash("Sản phẩm không tồn tại", "succsess");
                return RedirectToAction("Index");
            }
            menu.menu_status = (menu.menu_status == 1) ? 2 : 1;
            menu.menu_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            var list = db.C_menu.Where(m => m.menu_status == 2);
            return View(list.ToList());
        }
        // GET: Admin/Product/Delete/5

        public ActionResult Status(int id)
        {
            C_menu menu = db.C_menu.Find(id);
            if (menu == null)
            {
                Thongbao.set_flash("Loại sản phẩm không tồn tại", "succsess");
                return RedirectToAction("Index");
            }
            menu.menu_status = (menu.menu_status == 1) ? 0 : 1;
            menu.menu_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
