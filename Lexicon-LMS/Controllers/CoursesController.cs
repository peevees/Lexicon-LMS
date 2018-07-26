using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Lexicon_LMS.Models;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Reflection;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
                if (upload != null && upload.ContentLength > 0)
                {
                    var originalFilename = Path.GetFileName(upload.FileName);
                    string fileId = Guid.NewGuid().ToString().Replace("-", "");

                    var path = Path.Combine(Server.MapPath("~/Uploads"), originalFilename);
                    upload.SaveAs(path);

                    var file = new Document
                    {
                        Name = originalFilename,
                        UploadDate = DateTime.Now,
                        CourseID = course.ID,
                        Filepath = path,
                        User = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault()
                    };

                    course.CourseDocuments = new List<Document>();
                    course.CourseDocuments.Add(file);

                }

                course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult Download(string filePath, string fileName)
        {
            string fullName = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

            byte[] fileBytes = GetFile(fullName);
            return File(
                filePath,
                System.Net.Mime.MediaTypeNames.Application.Octet,
                Path.GetFileName(filePath)
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
        public ActionResult Edit([Bind(Include = "ID,CourseCode,CourseName,StartDate,EndDate,Description,TeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
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

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
    }
}
