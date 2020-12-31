using System;
using System.Linq;
using System.Web.Mvc;
using TTTN.Models;
namespace TTTN.Controllers
{
    public class DefaultController : Controller
    {
        TTTNdbContext db = new TTTNdbContext();
        // GET: Default
        public DefaultController()
        {
            if (System.Web.HttpContext.Current.Session["User_Id"] != null)
            {
                var us = db.C_user.Find(Convert.ToInt32(System.Web.HttpContext.Current.Session["User_Id"]));
                var orderr = db.C_order
                      .Where(m => m.order_status == 1 && m.order_userid == us.user_id);
                if (orderr.Count() > 0)
                {
                    var list = db.C_orderdetail
                                      .Where(m => m.orderdetail_orderid == orderr.FirstOrDefault().order_id).ToList();

                    if (System.Web.HttpContext.Current.Session["Sl_Cart"] != null)
                    {
                        if (list != null)
                        {
                            System.Web.HttpContext.Current.Session["Sl_Cart"] = list.Count();
                        }
                    }
                }

            }
        }
    }
}