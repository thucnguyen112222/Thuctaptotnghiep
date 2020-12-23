using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTTN.Models;
using System.IO;
namespace TTTN.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        TTTNdbContext db = new TTTNdbContext();
        // GET: Admin/Contact
        public ActionResult Index()
        {
            var List_contact = db.C_contact.OrderByDescending(m => m.contact_status);
            return View(List_contact.ToList());
        }

        public ActionResult Reply(int? id)
        {
            var List_contact = db.C_contact.OrderByDescending(m => m.contact_id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_contact contact = db.C_contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            if (contact.contact_status == 0)
            {
                Thongbao.set_flash("Đã trả lời liên hệ này", "danger");
                return RedirectToAction("Index", List_contact.ToList());
            }
            return View(contact);
        }
        [HttpPost]
        public ActionResult Reply(int id, FormCollection fiel)
        {
            var contact = db.C_contact.Find(id);
            var List_contact = db.C_contact.AsNoTracking().OrderByDescending(m => m.contact_id);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/SendMail.html"));
            string path = Server.MapPath("~/Views/logo.jpg");
           
            content = content.Replace("{{img}}", path);
            content = content.Replace("{{fullname}}", contact.contact_fullname);
            content = content.Replace("{{Phone}}", contact.contact_phone);
            content = content.Replace("{{Email}}", contact.contact_email);
            content = content.Replace("{{Detail}}", fiel["reply"]);
            Mail.SendMail(contact.contact_email, content,"Liên Hệ","Trả Lời Liên Hệ");

            contact.contact_status = 0;
            contact.contact_updatedat = DateTime.Now;
            contact.contact_updatedby = Convert.ToInt32(Session["User_Id_Admin"].ToString());
            db.SaveChanges();
            Thongbao.set_flash("Trả lời thành công", "success");

            return View("Index", List_contact.ToList());
        }
    }
}