using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTTN.Models;
using PagedList;
using System.IO;

namespace TTTN.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private TTTNdbContext db = new TTTNdbContext();

        // GET: Admin/Post
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var list_post = db.C_post.Where(m => m.post_status != 2)
                .OrderByDescending(m => m.post_id)
                .ToPagedList(pageNumber, pageSize);
            return View(list_post);
        }
        //thùng rác
        public ActionResult Trash()
        {
            var list_post = db.C_post.Where(m => m.post_status == 2)
                .OrderByDescending(m => m.post_id).ToList();
            return View(list_post);
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_post c_post = db.C_post.Find(id);
            if (c_post == null)
            {
                return HttpNotFound();
            }
            return View(c_post);
        }

        // GET: Admin/Post/Create
        public ActionResult Create()
        {
            ViewBag.ListTop = db.C_topic.Where(m => m.topic_status != 2).ToList();
            return View();
        }

        // POST: Admin/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(C_post post, HttpPostedFileBase file)
        {
            ViewBag.ListTop = db.C_topic.Where(m => m.topic_status != 2).ToList();

            if (!ModelState.IsValid)
            {
                string slug = Xstring.ToAscii(post.post_title);
                var count_item = db.C_post.Where(m => m.post_slug == slug);
                if (count_item.Count() > 0)
                {
                    Thongbao.set_flash("Bài viết đã tồn tại", "danger");
                    return RedirectToAction("Index");
                }
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName.Length < 255)
                    {
                        post.post_img = fileName;
                        string path = Path.Combine(Server.MapPath("~/wwwroot/img/post"), fileName);
                        file.SaveAs(path);
                    }
                    else
                    {
                        Thongbao.set_flash("Tên hình ảnh không vượt quá 255 kí tự", "danger");
                        return RedirectToAction("Index");
                    }
                }
                post.post_slug = slug;
                post.post_detail = post.post_detail;
                post.post_createdat = DateTime.Now;
                post.post_createdby = Convert.ToInt32(Session["User_Admin_Id"]);
                db.C_post.Add(post);
                db.SaveChanges();
                C_link link = new C_link();
                link.slug = slug;
                link.type = "post";
                link.tableid = post.post_id;
                db.C_link.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }
        // GET: Admin/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListTop = db.C_topic.Where(m => m.topic_status != 2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_post c_post = db.C_post.Find(id);
            if (c_post == null)
            {
                return HttpNotFound();
            }
            return View(c_post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(C_post post, int id, HttpPostedFileBase file)
        {
            ViewBag.ListTop = db.C_topic.Where(m => m.topic_status != 2);
            var item = db.C_post.Find(id);
            var link = db.C_link
                 .Where(m => m.slug == item.post_slug && m.tableid == item.post_id && m.type == "post").First();

            if (!ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName.Length < 255)
                    {
                        post.post_img = fileName;
                        string path = Path.Combine(Server.MapPath("~/wwwroot/img/post"), fileName);
                        file.SaveAs(path);
                    }
                    else
                    {
                        Thongbao.set_flash("Tên hình ảnh không vượt quá 255 kí tự", "danger");
                        return RedirectToAction("Index");
                    }
                }
                string slug = Xstring.ToAscii(post.post_title);
                var count_item = db.C_post.Where(m => m.post_slug == slug);
                item.post_status = post.post_status;
                item.post_topid = post.post_topid;
                item.post_type = post.post_type;
                item.post_metakey = post.post_metakey;
                item.post_metadesc = post.post_metadesc;
                item.post_title = post.post_title;
                item.post_slug = slug;
                item.post_detail = post.post_detail;
                item.post_updatedat = DateTime.Now;
                item.post_updatedby = Convert.ToInt32(Session["User_Admin_Id"]);
                db.Entry(item).State = EntityState.Modified;

                 link.slug = slug;
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int id)
        {
            C_post c_post = db.C_post.Find(id);
            db.C_post.Remove(c_post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            C_post post = db.C_post.Find(id);
            if (post == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            post.post_status = 2;
            post.post_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã chuyển vào thùng rác", "success");
            return RedirectToAction("Index");
        }
        public ActionResult ReTrash(int? id)
        {
            C_post post = db.C_post.Find(id);
            if (post == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            post.post_status = 0;
            post.post_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Đã khôi phục", "success");
            return RedirectToAction("Index");
        }
        public ActionResult Status(int id)
        {
            C_post post = db.C_post.Find(id);
            if (post == null)
            {
                Thongbao.set_flash("Chủ đề không tồn tại", "danger");
                return RedirectToAction("Index");
            }
            post.post_status = (post.post_status == 1) ? 0 : 1;
            post.post_updatedat = DateTime.Now;

            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
    }
}
