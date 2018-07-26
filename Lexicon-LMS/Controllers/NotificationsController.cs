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
            return View(notification);
        }

        // GET: Notifications/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(string id)
        {
            if(id!=null)
            {
                ViewBag.recipient = db.Users.Find(id);
            }

                return View();

        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "ID,Subject,Body,RecipientID")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.Sender = db.Users.Find(User.Identity.GetUserId());
                notification.DateSent = DateTime.Now;

                var recipientslist = notification.RecipientID.Split( new char[] {',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(var id in recipientslist)
                {
                    var recipient = db.Users.Find(id);
                    recipient.Notifications.OrderByDescending(rec => rec.DateSent);
                }

                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
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
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("Index");
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
