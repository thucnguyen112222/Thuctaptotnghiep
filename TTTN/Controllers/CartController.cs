using System;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Controllers
{
    public class CartController : DefaultController
    {
        private TTTNdbContext db = new TTTNdbContext();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                var us = db.C_user.Find(Convert.ToInt32(Session["User_Id"]));

                var orderr = db.C_order
                      .Where(m => m.order_status == 1 && m.order_userid == us.user_id);

                var list = db.C_orderdetail
                    .Where(m => m.orderdetail_orderid == orderr.FirstOrDefault().order_id).ToList();

                ViewBag.tt = 0;
                if (list != null)
                {
                    ViewBag.tt = list.Count();
                    Session["SL_Cart"] = list.Count();
                }
                return View(list);
            }
            else
            {
                return Redirect(url: "~/user/dang-nhap");
            }
        }
        public ActionResult AddItem(int Id)
        {
            if (Session["user"] == null)
            {
                return Redirect("~/user/dang-nhap");
            }
            
            var us = db.C_user.Find(Convert.ToInt32(Session["User_Id"]));

            var sp = db.C_product.Find(Id);
            if (sp.product_number <= 0)
            {
                Thongbao.set_flash("Sản phẩm tạm thời hết hàng", "danger");
                return RedirectToAction("Index");
            }
            var orderr = db.C_order
                    .Where(m => m.order_status == 1 && m.order_userid == us.user_id);
            if (orderr.Count() == 0)
            {
                C_order order = new C_order()
                {
                    order_code = DateTime.Now.ToString("ddMMyyhms"),
                    order_userid = us.user_id,
                    order_createdate = DateTime.Now,
                    order_deliveryaddress = us.user_address,
                    order_deliveryname = us.user_fullname,
                    order_deliveryphone = us.user_phone,
                    order_email = us.user_email,
                    order_createdby = us.user_id,
                    order_status = 1,
                    order_name = us.user_fullname,
                };
                db.C_order.Add(order);

                C_orderdetail orderdetail = new C_orderdetail()
                {
                    orderdetail_orderid = order.order_id,
                    orderdetail_productid = sp.product_id,
                    orderdetail_price = (int)(sp.product_pricesale != 0 ? sp.product_pricesale : sp.product_price),
                    orderdetail_quanity = 1,
                    orderdetail_amount = (int)(sp.product_pricesale != 0 ? sp.product_pricesale : sp.product_price),
                    orderdetail_name = sp.product_name,
                    orderdetail_img = sp.product_img,
                };
                db.C_orderdetail.Add(orderdetail);
                db.SaveChanges();
                return Redirect(url: "~/gio-hang");
            }

            var list = db.C_orderdetail
               .Where(m => m.orderdetail_orderid == orderr.FirstOrDefault().order_id && m.orderdetail_productid == sp.product_id);

            if (list.Count() > 0)
            {
                if (list.First().orderdetail_quanity < 5)
                {
                    list.First().orderdetail_quanity++;
                    list.First().orderdetail_total = (int)list.First().orderdetail_price * list.First().orderdetail_quanity;
                    db.SaveChanges();
                    return Redirect("~/gio-hang");
                }
                else
                {
                    Thongbao.set_flash("Thất bại", "danger");
                    return Redirect("~/gio-hang");
                }
            }
            else
            {
                C_orderdetail orderdetail = new C_orderdetail()
                {
                    orderdetail_orderid = orderr.FirstOrDefault().order_id,
                    orderdetail_productid = sp.product_id,
                    orderdetail_price = ((int)(sp.product_pricesale != 0 ? sp.product_pricesale : sp.product_price)),
                    orderdetail_quanity = 1,
                    orderdetail_amount = ((int)(sp.product_pricesale != 0 ? sp.product_pricesale : sp.product_price)),
                    orderdetail_name = sp.product_name,
                    orderdetail_img = sp.product_img,
                    orderdetail_total = ((int)(sp.product_pricesale != 0 ? sp.product_pricesale : sp.product_price))
                };
                db.C_orderdetail.Add(orderdetail);
                db.SaveChanges();
                return Redirect("~/gio-hang");
            }
        }
        public ActionResult UpdatePlus(int Id)
        {

            var ItemUpdate = db.C_orderdetail.Find(Id);
            if (ItemUpdate.orderdetail_quanity < 5)
            {
                ItemUpdate.orderdetail_quanity++;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Thongbao.set_flash("Thất bại", "danger");

                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateMinus(int Id)
        {
            var ItemUpdate = db.C_orderdetail.Find(Id);

            if (ItemUpdate.orderdetail_quanity > 1)
            {
                ItemUpdate.orderdetail_quanity--;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Thongbao.set_flash("Thất bại", "danger");

                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(int Id)
        {
            var ItemUpdate = db.C_orderdetail.Find(Id);
            db.C_orderdetail.Remove(ItemUpdate);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "danger");
            return RedirectToAction("Index");
        }
        public ActionResult PayMent()
        {


            if (Session["User_Id"] == null)
            {
                return Redirect("~/user/dang-nhap");
            }
            else
            {
                int userID = Convert.ToInt32(Session["User_Id"].ToString());
                var order = db.C_order
                    .Where(m => m.order_status == 1 && m.order_userid == userID);
                if (order.Count() == 0)
                {
                    ViewBag.UserOrder = null;
                }
                else
                {
                    ViewBag.UserOrder = order.First();
                    ViewBag.CodeOrder = order.First();
                }
                var orderdetail = db.C_orderdetail.Where(m => m.orderdetail_orderid == order.FirstOrDefault().order_id).ToList();
                return View(orderdetail);
            }
        }
        [HttpPost]
        public ActionResult PayMent(FormCollection form)
        {
            if (Session["User_Id"] == null)
            {
                return Redirect("~/user/dang-nhap");
            }

            int userID = Convert.ToInt32(Session["User_Id"].ToString());
            var _Order = db.C_order.Where(m => m.order_userid == userID).First();
            ViewBag.CodeOrder = _Order;
            string name = form["name"];
            string address = form["address"];
            string phone = form["phone"];
            string email = form["email"];
            var payment = (form["payment"]);
            var arrVal = payment.Split(',');
            int payment_item = Convert.ToInt32(arrVal.First());
            if (payment_item == 1)
            {
                _Order.order_name = name;
                _Order.order_deliveryaddress = address;
                _Order.order_deliveryphone = phone;
                _Order.order_email = email;
                _Order.order_payment = payment_item;
                _Order.order_status = 0;
                _Order.order_order = DateTime.Now;
                db.SaveChanges();
                Thongbao.set_flash("Đặt hàng thành công", "success");
                ViewBag.pay = payment_item;
                return View();
            }
            _Order.order_name = name;
            _Order.order_deliveryaddress = address;
            _Order.order_deliveryphone = phone;
            _Order.order_email = email;
            _Order.order_status = 0;
            db.SaveChanges();
            ViewBag.pay = payment_item;
            return View();
        }
        public ActionResult paymentBanking()
        {
            if (Session["User_Id"] == null)
            {
                return Redirect("~/user/dang-nhap");
            }
            int userID = Convert.ToInt32(Session["User_Id"].ToString());
            var _Order = db.C_order.Where(m => m.order_userid == userID).First();
            _Order.order_payment = 2;
            _Order.order_status = 0;
            _Order.order_order = DateTime.Now;
            db.SaveChanges();
            Thongbao.set_flash("Đặt hàng thành công", "success");
            return View("Payment");
        }
    }
}