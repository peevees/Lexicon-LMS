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

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
                model.CourseCode = targetCourse.ID;
                model.Course = targetCourse;
            }
            

            return View(model);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "ID,StartDate,EndDate,Description,CourseCode")] Module module, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                
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
                        ModuleID = module.ID,
                        Filepath = path,
                        User = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault()
                    };

                    module.Documents = new List<Document>();
                    module.Documents.Add(file);

                }

                Course targetCourse = db.Courses.Where(course => course.ID == module.CourseCode).FirstOrDefault();
                targetCourse.CourseModules.Add(module);


                //course.Teacher = db.Users.Where(u => u.Id == course.TeacherID).FirstOrDefault();
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCode = new SelectList(db.Courses, "ID", "CourseCode", module.CourseCode);
            return View(module);
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
                return RedirectToAction("Index");
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
