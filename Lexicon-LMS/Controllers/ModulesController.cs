using Lexicon_LMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FileHandler fileHandler = new FileHandler();

        // GET: Modules
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int? id)
        {
            //ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseName");

            Course targetCourse = db.Courses.Where(course => course.ID == id).FirstOrDefault();
            ViewBag.CourseLabel = targetCourse.CourseName + " (" + targetCourse.CourseCode + ")";   // TODO: cannot create module at the moment
            Module model = new Module();
            if (id != null)
            {
                model.CourseCode = targetCourse.CourseCode;
                model.Course = targetCourse;
                model.StartDate = DateTime.Today;
                model.EndDate = DateTime.Today.AddDays(1);
            }


            return View(model);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "ID,StartDate,EndDate,Description,CourseCode,ModuleTitle")] Module module, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                var file = fileHandler.UploadFile(upload);

                if (file != null)
                {
                    var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    file.ModuleID = module.ID;
                    file.User = user;

                    module.Documents = new List<Document>();
                    module.Documents.Add(file);
                }

                Course targetCourse = db.Courses.Where(course => course.CourseCode == module.CourseCode).FirstOrDefault();
                targetCourse.CourseModules.Add(module);
                db.Modules.Add(module);
                db.SaveChanges();
            }

            ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseCode", module.CourseCode);
            return RedirectToAction("Details", "Courses", new { id = module.Course.ID });
        }

        [Authorize]
        public ActionResult Download(string filePath, string fileName)
        {
            var file = fileHandler.DownloadFile(filePath, fileName);
            if (file == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File was not found");
            }

            return file;


            //string fullName = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

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

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseCode", module.CourseCode);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "ID,StartDate,EndDate,Description,CourseCode")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                module.Course = db.Courses.Where(c => c.CourseCode == module.CourseCode).FirstOrDefault();
                return RedirectToAction("Details", "Courses", new { id = module.Course.ID });
            }
            ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseCode", module.CourseCode);
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            module.Course = db.Courses.Where(c => c.CourseCode == module.CourseCode).FirstOrDefault();
            return RedirectToAction("Details", "Courses", new { id = module.Course.ID });
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
