using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notifications
        public ActionResult Index()
        {
            return View(db.Notifications.ToList());
        }

        public ActionResult InboxContainer()
        {
            return PartialView();
        }

        public ActionResult Inbox(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return PartialView(user.Notifications.OrderByDescending(l => l.DateSent).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult InboxMsg(int notif)
        {

            var model = db.Notifications.Find(notif);
            model.Read = true;
            db.SaveChanges();

            return PartialView(model);
        }


        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return PartialView(notification);
        }

        // GET: Notifications/Create
        public ActionResult Create(string uid = null, string subject = null, string body =null)
        {
            Notification model = new Notification();
            if (uid!=null)
            {
                ViewBag.recipient = db.Users.Find(uid);
                model.RecipientID = uid;
                model.Subject = subject;
            }

                return View(model);

        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,Body,RecipientID")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.Sender = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                notification.DateSent = DateTime.Now;
                notification.Recipients = new List<ApplicationUser>();

                var recipientslist = notification.RecipientID.Split( new char[] {',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(var id in recipientslist)
                {
                    var recipient = db.Users.Find(id);
                    notification.Recipients.Add(recipient);
                    recipient.Notifications.Add(notification);
                    recipient.Notifications.OrderBy(rec => rec.DateSent);
                }

                 db.Notifications.Add(notification);
                db.SaveChanges();
                return Redirect("/Manage/Index?tab=inbox");
            }

            return View(notification);
        }

        // GET: Notifications/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "ID,Subject,Body,Read")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id, int? page)
        {
            Notification notification = db.Notifications.Find(id);
            foreach(var rec in notification.Recipients)
            {
                rec.Notifications.Remove(notification);
            }
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("Index","Manage", new {tab = "inbox", p = page });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetUsers(string q)
        {
            var users = db.Users.Where(c => (c.Forename + " " + c.Surname).StartsWith(q));
            //^^ Can't use non-assigned properties like FullName in LINQ so that's why it's like that

            if (User.IsInRole("Teacher"))
            {
                return Json(users.Select(a => new { name = a.Forename + " " + a.Surname, id = a.Id }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                ApplicationUser student = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                return Json(users.Where(c=>c.UserCourse == student.UserCourse).Select(a => new { id = a.Id, name = a.Forename + " " + a.Surname }), JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}
