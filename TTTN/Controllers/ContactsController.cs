using System;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Controllers
{
    public class ContactsController : DefaultController
    {
        private TTTNdbContext db = new TTTNdbContext();
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.contact_createdat = DateTime.Now;
                contact.contact_status = 1;
                db.C_contact.Add(contact);
                db.SaveChanges();
                Thongbao.set_flash("Cảm ơn bạn đã đóng góp ý kiến chúng tôi sẽ sớm phản hồi ý kiến của bạn !", "success");
            }
            else
            {
                Thongbao.set_flash("Thất bại", "danger");
            }
            return View(contact);
        }
    }
}