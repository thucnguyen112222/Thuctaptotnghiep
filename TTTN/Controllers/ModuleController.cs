using System.Linq;
using System.Web.Mvc;
using TTTN.Models;
namespace TTTN.Controllers
{
    public class ModuleController : DefaultController
    {
        TTTNdbContext db = new TTTNdbContext();
        public ActionResult Slider()
        {
            var slider = db.C_slider
                .Where(m => m.slider_position == "Slideshow" && m.slider_status == 1)
                .OrderByDescending(m => m.slider_order).ToList();
            return View(slider);
        }
        public ActionResult Menu()
        {
            var menulisst = db.C_menu.Where
                (m => m.menu_status == 1 && m.menu_parentid == 0 && m.menu_position == "menu")
                .OrderBy(m => m.menu_id);

            return View(menulisst.ToList());
        }
        public ActionResult SubMenu(int parentid)
        {
            ViewBag.menuitem = db.C_menu.Find(parentid);
            var menu = db.C_menu
                .Where(m => m.menu_status == 1 && m.menu_parentid == parentid && m.menu_position == "menu");
            if (menu.Count() != 0)
            {
                return View("Submenu1", menu.ToList());
            }
            else
            {
                return View("SubMenu", menu);
            }
        }
        public ActionResult SubMenu2(int parentid)
        {
            ViewBag.menuitem2 = db.C_menu.Find(parentid);
            var menu2 = db.C_menu
                .Where(m => m.menu_status == 1 && m.menu_parentid == parentid && m.menu_position == "menu");
            if (menu2.Count() != 0)
            {
                return View("SubMenu2", menu2.ToList());
            }
            else
            {
                return View("SubMenu3");
            }
        }
        public ActionResult Category()
        {
            var Cat = db.C_category
                .Where(m => m.category_status == 1 && m.category_parentid == 0).ToList();
            return View(Cat);
        }
        public ActionResult SubCategory(int parentid)
        {
            ViewBag.subcategory = db.C_category.Find(parentid);
            var Cat = db.C_category
                .Where(m => m.category_status == 1 && m.category_parentid == parentid).ToList();
            if (Cat.Count() != 0)
            {
                return View("SubCategory", Cat);
            }
            else
            {
                return View("SubCategory2");
            }
        }
        public ActionResult ProductModule()
        {
            var list = db.C_orderdetail.Select(m=>m.orderdetail_productid);
            var product_item = db.C_product
                .Where(m=>m.product_status ==1 && list.Contains(m.product_id)).Take(3);
            return View(product_item);
        }
        public ActionResult PostModule()
        {
            var list = db.C_post
                .Where(m=>m.post_status ==1 && m.post_type =="post")
                .OrderByDescending(m=>m.post_createdat)
                .Take(3).ToList();
            return View(list);
        }
    }
}