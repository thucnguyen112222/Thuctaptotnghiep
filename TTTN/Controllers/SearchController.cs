using PagedList;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Controllers
{
    public class SearchController : DefaultController
    {
        TTTNdbContext db = new TTTNdbContext();
        // GET: Search
        public ActionResult Index(string searchString, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            ViewBag.Keyword = searchString;
            var products = db.C_product
                .Where(m => m.product_name.Contains(searchString) && m.product_status == 1)
                .OrderByDescending(m => m.product_createdat).ToList();
            ViewBag.search = products.Count();
            if (products.Count() == 0)
            {
                Thongbao.set_flash("Không tìm thấy sản phẩm", "danger");
            }
            return View("Index", products.ToPagedList(pageNumber, pageSize));
        }
    }
}