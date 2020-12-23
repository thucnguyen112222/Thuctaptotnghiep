using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTTN.Models;

namespace TTTN.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Topic
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var list_topic = db.C_topic
                .Where(m => m.topic_status != 2)
                .OrderByDescending(m => m.topic_id);
            return View(list_topic.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_topic c_topic = db.C_topic.Find(id);
            if (c_topic == null)
            {
                return HttpNotFound();
            }
            return View(c_topic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            ViewBag.List = db.C_topic.Where(m => m.topic_status != 2);
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_topic topic)
        {
            ViewBag.List = db.C_topic.Where(m => m.topic_status != 2);
            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(topic.topic_name);
                var countItem = db.C_topic.Where(m => m.topic_slug == slug);
                if (slug.Length > 255)
                {
                    Thongbao.set_flash("Chủ đề không được quá 255 kí tự","danger");
                    return RedirectToAction("Index");
                }
                if (countItem.Count() > 0)
                {
                    Thongbao.set_flash("Chủ đề đã tồn tại", "danger");
                    return RedirectToAction("index");
                }
                topic.topic_slug = slug;
                topic.topic_createdat = DateTime.Now;
                topic.topic_createdby = Convert.ToInt32(Session["User_Admin_Id"]);
                db.C_topic.Add(topic);
                C_link link = new C_link();
                link.slug = slug;
                link.tableid = topic.topic_id;
                link.type = "topic";
                db.C_link.Add(link);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.topic_edit = db.C_topic.Where(m => m.topic_status != 2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_topic c_topic = db.C_topic.Find(id);
            if (c_topic == null)
            {
                Thongbao.set_flash("", "");
                return RedirectToAction("Index");

            }
            return View(c_topic);
        }

        // POST: Admin/Topic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(C_topic topic, int id)
        {
            ViewBag.topic_edit = db.C_topic.Where(m => m.topic_status != 2);

            if (!ModelState.IsValid)
            {
                var topic_item = db.C_topic.Find(id);
                var link = db.C_link.Where(m => m.tableid == topic.topic_id && m.slug == topic_item.topic_slug).First();

                string slug = Xstring.ToAscii(topic.topic_name);
                if (slug.Length > 255)
                {
                    Thongbao.set_flash("Chủ đề không được quá 255 kí tự", "danger");
                    return RedirectToAction("Index");
                }
                var countItem = db.C_topic.Where(m => m.topic_slug == slug);
                if (countItem.Count() > 0)
                {
                    Thongbao.set_flash("Chủ đề đã tồn tại", "danger");
                    return RedirectToAction("index");
                }
                topic_item.topic_slug = slug;
                topic_item.topic_name = topic.topic_name;
                topic_item.topic_metadesc = topic.topic_metadesc;
                topic_item.topic_metakey = topic.topic_metakey;
                topic_item.topic_updatedat = DateTime.Now;
                topic_item.topic_updatedby = Convert.ToInt32(Session["User_Admin_Id"]);
                db.Entry(topic_item).State = EntityState.Modified;
                link.slug = topic.topic_slug;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        public ActionResult DelTrash(int? id)
        {
            C_topic topic = db.C_topic.Find(id);
            if (topic == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            topic.topic_status = 2;
            topic.topic_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }
        public ActionResult ReTrash(int? id)
        {
            C_topic topic = db.C_topic.Find(id);
            if (topic == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            topic.topic_status = 0;
            topic.topic_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã khôi phục", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            var list = db.C_topic.Where(m => m.topic_status == 2);
            return View(list.ToList());
        }


        // GET: Admin/topic/Delete/5
        public ActionResult Delete(int id)
        {
            C_topic c_topic = db.C_topic.Find(id);
            var item_link = db.C_link.Where(m => m.slug == c_topic.topic_slug).First();
            db.C_link.Remove(item_link);
            db.C_topic.Remove(c_topic);
            db.SaveChanges();
            Thongbao.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Status(int id)
        {
            C_topic topic = db.C_topic.Find(id);
            if (topic == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            topic.topic_status = (topic.topic_status == 1) ? 0 : 1;
            topic.topic_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }

    }
}
