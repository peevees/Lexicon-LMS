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
        public Document Upload(HttpPostedFileBase upload, int courseID)
        {
            Course targetCourse = db.Courses.Where(c => c.ID == courseID).FirstOrDefault();

            var originalFilename = Path.GetFileName(upload.FileName);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            string fileId = Guid.NewGuid().ToString().Replace("-", "");
            var fileName = user.Forename + "_" + user.Surname + "_" + fileId + "_" + originalFilename;
            var path = Path.Combine(Server.MapPath("~/Uploads/"), targetCourse.CourseName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var file = new Document
            {
                FileName = fileName,
                DisplayName = originalFilename,
                UploadDate = DateTime.Now,
                CourseID = courseID,
                Filepath = path,
                User = db.Users.Find(User.Identity.GetUserId())
            };

            upload.SaveAs(Path.Combine(path, fileName));
            targetCourse.Documents.Add(file);

            return (file);
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
            db.Courses.Remove(course);
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

        public ActionResult UploadCourseDoc(int? id)
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
            return PartialView(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCourseDoc([Bind(Include = "ID")] Course course, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {

                Upload(upload, course.ID);
                
                db.SaveChanges();
                ViewBag.UploadStatus = "File uploaded successfully.";
                return RedirectToAction("Details", new { id = course.ID });
            }
            ModelState.AddModelError("", "No file uploaded");
            return RedirectToAction("Details", new { id = course.ID });
        }

        public ActionResult DeleteDoc(int id)
        {
            Document doc = db.Documents.Find(id);
            var tgtCourse = db.Courses.Find(doc.CourseID);
            string fullPath = doc.Filepath + "/"+doc.FileName;

            tgtCourse.Documents.Remove(doc);
            System.IO.File.Delete(fullPath);
            db.Documents.Remove(doc);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = tgtCourse.ID });
        }
    }
}
