using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Order
        public ActionResult Index()
        {
            var lits_order = db.C_order
                .Where(m => m.order_status != 1).ToList()
                .OrderBy(m => m.order_status);
            return View(lits_order);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_order order = db.C_order.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            var list_product = db.C_orderdetail.Where(m => m.orderdetail_orderid == order.order_id).ToList();
            int total = 0;
            foreach (var i in list_product)
            {
                total += (int)i.orderdetail_quanity * (int)i.orderdetail_price;
            }
            ViewBag.Total = total;
            ViewBag.list_product = list_product;
            return View(order);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            C_order c_order = db.C_order.Find(id);
            db.C_order.Remove(c_order);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "success");
            var lits_order = db.C_order
               .Where(m => m.order_status != 1).ToList()
               .OrderByDescending(m => m.order_createdate);
            return View("Index", lits_order);
        }
        public ActionResult Status(int id)
        {
            C_order order = db.C_order.Find(id);
            if (order == null)
            {
                Thongbao.set_flash("Đơn hàng không tồn tại", "danger");
                return RedirectToAction("Index", "Order");
            }
            order.order_status = 2;
            order.order_order = DateTime.Now;
            order.order_updatedat = DateTime.Now;
            //sendmail
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/CartOrder.html"));
            content = content.Replace("{{fullname}}", order.order_name);
            content = content.Replace("{{OrderCode}}", order.order_code);
           
            Mail.SendMail(order.order_email, content,"Đơn Hàng","Đơn đặt hàng");
            db.SaveChanges();
            Thongbao.set_flash("Thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
