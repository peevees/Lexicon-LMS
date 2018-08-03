using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index()
        {
            return View(db.Activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int? id)
        {
            Module targetModule = db.Modules.Where(module => module.ID == id).FirstOrDefault();
            ViewBag.ModuleLabel = targetModule.Course.CourseName + " (" + targetModule.Course.CourseCode + ") - " + targetModule.Description;
            Activity model = new Activity();
            if (id != null)
            {
                model.ModuleID = targetModule.ID;
            }
            return View(model);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "ID,Name,Deadline,ModuleID")] Activity activity, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                Module targetModule = db.Modules.Where(module => module.ID == activity.ModuleID).FirstOrDefault();
                activity.Module = targetModule;


                if (upload != null && upload.ContentLength > 0)
                {
                    var originalFilename = Path.GetFileName(upload.FileName);
                    var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                    string fileId = Guid.NewGuid().ToString().Replace("-", "");

                    var fileName = user.Forename + "_" + user.Surname + "_" + fileId + "_" + originalFilename;

                    var path = Path.Combine(Server.MapPath("~/Uploads"));
                    var save = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    upload.SaveAs(save);

                    var file = new Document
                    {
                        FileName = fileName,
                        DisplayName = originalFilename,
                        UploadDate = DateTime.Now,
                        ActivityID = activity.ID,
                        Filepath = path,
                        User = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault()
                    };

                    activity.Documents = new List<Document>();
                    activity.Documents.Add(file);

                }

                if (Request["notify"] != null)
                {
                    foreach (ApplicationUser u in activity.Module.Course.CourseParticipants)
                    {
                        var alert = NewActivityAlert(activity, db.Users.Find(User.Identity.GetUserId()));
                        alert.RecipientID = u.Id;
                        u.Notifications.Add(alert);
                    }
                }

                //course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();
                targetModule.ModuleActivities.Add(activity);
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseCode", module.CourseCode);
            return View(activity);
        }

        [Authorize]
        public ActionResult Download(string filePath, string fileName, string saveName)
        {
            string fullName = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

            if (!System.IO.File.Exists(fullName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File was not found");
            }

            string contentType = MimeMapping.GetMimeMapping(filePath);
            byte[] fileBytes = GetFile(fullName);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = saveName,
                Inline = false
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(
             fileBytes,
             contentType
             );
        }

        private byte[] GetFile(string fullName)
        {

            //is null check filepath
            //https://stackoverflow.com/questions/3597179/file-download-in-asp-net-mvc-2

            FileStream fs = System.IO.File.OpenRead(fullName);

            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new IOException(fullName);

            return data;
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "ID,Name,Deadline,ModuleID")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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

        public Notification NewActivityAlert(Activity activity, ApplicationUser user)
        {
            var notif = new Notification();
            notif.Subject = user.FullName + " has added a new activity";
            notif.Body = "A new activity (" + activity.Name + ") has been added to \"" + activity.Module.Description + "\" module in " + activity.Module.Course.CourseName;
            notif.Sender = user;
            notif.Recipients = activity.Module.Course.CourseParticipants.ToList();
            notif.DateSent = DateTime.Now;

            return notif;
        }

    }
}
