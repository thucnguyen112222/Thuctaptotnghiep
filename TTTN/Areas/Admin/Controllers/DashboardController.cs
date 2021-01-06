using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;
namespace TTTN.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        TTTNdbContext db = new TTTNdbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            int i = Convert.ToInt32(Session["User_Id_admin"]);
            ViewBag.User_Profile = db.C_user.Where(m => m.user_id == i).First();
            ViewBag.list_order = OrderNew();
            ViewBag.Gia = Profit();
            ViewBag.NewContact = ContactNew();
            ViewBag.Countproduct = db.C_product.ToList().Count();
            ViewBag.CountCategory = db.C_category.ToList().Count();
            ViewBag.Countpost = db.C_post.ToList().Count();
            ViewBag.selling_products = selling_products();
            return View();
        }
        public List<C_orderdetail> selling_products()
        {
            var listproduct = db.C_orderdetail.ToList();
            var list = new List<C_orderdetail>();

            foreach (var i in listproduct)
            {
                if (list.Count() <= 0)
                {
                    list.Add(i);
                }
                else
                {
                    var iii = list.FirstOrDefault(m => m.orderdetail_productid == i.orderdetail_productid);
                    if (iii == null)
                    {
                        list.Add(i);
                    }
                    else
                    {
                        iii.orderdetail_quanity += i.orderdetail_quanity;
                    }
                }
            }
            return list.OrderByDescending(m => m.orderdetail_quanity).Take(5).ToList();
        }
        public int ContactNew()
        {
            var item = db.C_contact.Where(m => m.contact_status == 1).ToList();
            return item.Count();
        }
        public int OrderNew()
        {
            int list_order = db.C_order.Where(m => m.order_status == 0).Count();
            return list_order;
        }
        public int Profit()
        {
            var query = from o in db.C_order
                        join d in db.C_orderdetail
                        on o.order_id equals d.orderdetail_orderid
                        select new
                        {
                            o,
                            d
                        };
            query.ToList();
            int gia = 0;
            foreach (var item in query)
            {
                if (item.o.order_status == 2)
                {
                    int idproduct = item.d.orderdetail_productid;
                    var productID = db.C_product.Find(idproduct);
                    int giaban = item.d.orderdetail_quanity * item.d.orderdetail_price;
                    int gianhap = (int)(item.d.orderdetail_quanity * productID.product_importprice);
                    gia = giaban - gianhap;
                }
            }
            return gia;
        }
    }
}