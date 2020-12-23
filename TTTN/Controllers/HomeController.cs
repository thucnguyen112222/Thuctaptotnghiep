using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;


/// <summary>
/// https://www.c-sharpcorner.com/article/create-a-pdf-file-and-download-using-Asp-Net-mvc/
/// </summary>
namespace TTTN.Controllers
{
    public class HomeController : DefaultController
    {
        TTTNdbContext db = new TTTNdbContext();
        public ActionResult Index(String slug = "")
        {
            int? page = null;

            if (slug == "")
            {
                return View("Home");
            }
            else
            {
                var row_link = db.C_link.Where(m => m.slug == slug);

                if (row_link.Count() != 0)
                {
                    var link = row_link.First();
                    string type = link.type;
                    switch (type)
                    {
                        case "category": { return this.ProductCategory(slug, page); }
                        case "topic": { return this.ListTopic(slug); }
                        case "post": { return this.Post(slug); }
                        case "page": { return this.Page(slug); }
                        default: { return this.Error404(slug); }
                    }
                }
                else
                {
                    int count_product = db.C_product
                        .Where(m => m.product_slug == slug && m.product_status == 1).Count();
                    if (count_product != 0)
                    {
                        return this.ProductDetail(slug);
                    }
                    else
                    {
                        return this.Error404(slug);
                    }
                }
            }
        }
        public ActionResult ListTopic(string slug)
        {
            var item = db.C_topic.Where(m => m.topic_status == 1 && m.topic_slug == slug).First();
            var list_post = db.C_post.Where(m => m.post_status == 1 && m.post_topid == item.topic_id);
            ViewBag.item_topic = item.topic_name;
            return View("ListTopic", list_post.ToList());
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Error404(string slug)
        {
            return View("Error404");
        }
        public ActionResult NewProduct()
        {
            var list = db.C_product.Where
                (m => m.product_status == 1)
                .OrderByDescending(m => m.product_createdat)
                .ToList().Take(8);
            return View(list);
        }
        public ActionResult ProductSale()
        {
            var list = db.C_product.Where
                (m => m.product_status == 1 && m.product_pricesale != 0)
                .OrderByDescending(m => m.product_status == 1)
                .ToList().Take(4);

            return View(list);
        }
        public ActionResult ProductSaleAll(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var list = db.C_product.Where
                (m => m.product_status == 1 && m.product_pricesale != 0)
                .OrderByDescending(m => m.product_status == 1)
                .ToList();

            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductDetail(String slug)
        {
            var product_detail = db.C_product
                .Where(m => m.product_slug == slug && m.product_status == 1).First();
            var catname = db.C_category
                .Where(m => m.category_id == product_detail.product_catid && m.category_status == 1).First();
            ViewBag.loai = catname.category_name;
            return View("ProductDetail", product_detail);
        }
        public ActionResult ProductAll(int? page)
        {
            var list = (from l in db.C_product
                        select l).Where(x => x.product_status == 1).OrderBy(x => x.product_id);
            ViewBag.productall = list;
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductCategory(String slug, int? page)
        {
            var itemLink = db.C_link.Where(m => m.slug == slug && m.type == "category");
            if (itemLink.Count() <= 0)
            {
                return this.Index(slug);
            }
            var item = db.C_category
                .Where(m => m.category_slug == slug && m.category_status == 1).First();
            ViewBag.category = item.category_name;
            ViewBag.slugcategory = item.category_slug;
            var listcatid = db.C_category.Where(m => m.category_parentid == item.category_id).
                Select(m => m.category_id).ToList();

            if (page == null) page = 1;
            int pageSize =20;
            int pageNumber = (page ?? 1);

            var list = db.C_product
                         .Where(x => x.product_status == 1 && x.product_catid == item.category_id)
                         .OrderByDescending(x => x.product_id).ToList();
            return View("ProductCategory", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RelatedProduct(string slug)
        {
            var item = db.C_product.Where(m => m.product_slug == slug && m.product_status == 1).First();
            var listProduct = db.C_product
                .Where(m => m.product_status == 1 && m.product_catid == item.product_catid)
                .Take(3).ToList();
            return View(listProduct);
        }

        public ActionResult Topic()
        {
            var list_topic = db.C_topic.Where(m => m.topic_status == 1).ToList();

            var select_id = db.C_topic.Where(m => m.topic_status == 1).Select(m => m.topic_id);

            ViewBag.list_post = db.C_post.Where(m => m.post_status == 1 && select_id.Contains(m.post_topid)).Take(3);
            return View("Topic", list_topic);
        }
        public ActionResult Post(string slug)
        {
            var item_post = db.C_post.Where(m => m.post_status == 1 && m.post_slug == slug).First();
            return View("Post", model: item_post);
        }

        public ActionResult Page(string slug)
        {
            return View();
        }
    }
}
