using System;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;
namespace TTTN.Areas.Admin.Controllers
{
    public class SearchAdminController : Controller
    {
        TTTNdbContext db = new TTTNdbContext();
        public ActionResult Index(string searchstring, string item)
        {
            ViewBag.Item = item;
            if (item == "category")
            {
                var list = from m in db.C_category
                           select m;

                if (!String.IsNullOrEmpty(searchstring))
                {
                    ViewBag.Query = list.Where(s => s.category_name.Contains(searchstring));
                    return View("Index", list.ToList());
                }
                else
                {
                    Thongbao.set_flash("Không có kết quả", "danger");
                    return RedirectToAction("Index");
                }
            }
            if (item == "product")
            {
                var list = from m in db.C_product
                           select m;

                if (!String.IsNullOrEmpty(searchstring))
                {
                    ViewBag.Query = list.Where(s => s.product_name.Contains(searchstring));
                    return View("Index", list.ToList());
                }
                else
                {
                    Thongbao.set_flash("Không có kết quả", "danger");
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}