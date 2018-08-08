using Lexicon_LMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FileHandler fileHandler = new FileHandler();

        // GET: Courses
        //[Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {

            if (!User.IsInRole("Teacher"))
            {
                var currentUserCourse = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).UserCourse.ID;
                return RedirectToAction("Details", "Courses", new { id = currentUserCourse });
            }

            var courses = db.Courses.Include(c => c.Teacher).Where(c => c.EndDate >= DateTime.Now);
            return View(courses.ToList());
        }


        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);


            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            var role = db.Roles.SingleOrDefault(m => m.Name == "Teacher");
            var teachers = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            SelectList list = new SelectList(teachers, "Id", "FullName");
            ViewBag.teachers = list as IEnumerable<SelectListItem>;

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "ID,CourseCode,CourseName,StartDate,EndDate,Description,TeacherID")] Course course, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                var file = fileHandler.UploadFile(upload);
                if(file != null)
                {
                    var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    file.CourseID = course.ID;
                    file.User = user;

                    course.Documents = new List<Document>();
                    course.Documents.Add(file);
                }

                course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();

                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Authorize]
        public ActionResult Download(string filePath, string fileName)
        {
            var file = fileHandler.DownloadFile(filePath, fileName);
            if(file == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File was not found");
            }

            return file;

            //string fullName = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

            //if (!System.IO.File.Exists(fullName))
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File was not found");
            //}

            //string contentType = MimeMapping.GetMimeMapping(filePath);
            //byte[] fileBytes = GetFile(fullName);
            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = saveName,
            //    Inline = false
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(
            // fileBytes,
            // contentType

            // );
        }



        [Authorize]
        public ActionResult DeleteFile(int courseID, string filePath, string fileName, int documentID)
        {
            //TODO: maybe filehandler should handle delete?
            string fullName = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

            if (!System.IO.File.Exists(fullName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File was not found");
            }


            //Document document = db.Documents.Find(document2);

            Document document = db.Documents.Find(documentID);
            db.Documents.Remove(document);
            System.IO.File.Delete(fullName);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = courseID });

        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);

            var role = db.Roles.SingleOrDefault(m => m.Name == "Teacher");
            var teachers = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            SelectList list = new SelectList(teachers, "Id", "FullName");
            ViewBag.teachers = list as IEnumerable<SelectListItem>;

            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "ID,CourseCode,CourseName,StartDate,EndDate,Description,TeacherID")] Course course, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var file = fileHandler.UploadFile(upload);
                if (file != null)
                {
                    var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    file.CourseID = course.ID;
                    file.User = user;

                    db.Documents.Add(file);
                    db.SaveChanges();
                    course.Documents = new List<Document>();
                    course.Documents.Add(file);
                }

                course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");//TODO: should it really go to index?
            }

            return View(course);
        }

        // GET: Courses/Delete/5

        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {

            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
            */
            return RedirectToAction("DeleteConfirmed", id);
        }


        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            List<object> listTest = new List<object>();
            foreach (var document in course.Documents)
            {
                listTest.Add(document);
            }
            foreach (var module in course.CourseModules)
            {
                listTest.Add(module);
                //db.Entry(module).State = EntityState.Deleted;
                foreach (var document in module.Documents)
                {
                    listTest.Add(document);
                    //db.Entry(document).State = EntityState.Deleted;
                }
                foreach (var activity in module.ModuleActivities)
                {
                    listTest.Add(activity);
                    //db.Entry(activity).State = EntityState.Deleted;
                    foreach (var document in activity.Documents)
                    {
                        listTest.Add(document);
                        //db.Entry(document).State = EntityState.Deleted;
                    }
                }
            }
            foreach (var user in course.CourseParticipants)
            {
                listTest.Add(user);
                //db.Entry(user).State = EntityState.Deleted;
                foreach (var notification in user.Notifications)
                {
                    listTest.Add(notification);
                    //db.Entry(notification).State = EntityState.Deleted;
                }
            }



            foreach (var item in listTest)
            {
                db.Entry(item).State = EntityState.Deleted;

            }

            var userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.AddRange(db.Users.Where(u => u.UserCourse.ID == course.ID));
            foreach (var item in users)
            {
                item.UserCourseCode = null;
                item.UserCourse = null;
                //userManager.RemoveFromRoleAsync(item.Id, "Teacher");
                //userManager.DeleteAsync(item);
            }


            //db.Courses.Remove(course);
            db.Entry(course).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost, ActionName("Delete")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    Course course = await db.Courses.FindAsync(id);
        //    db.Courses.Remove(course);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
