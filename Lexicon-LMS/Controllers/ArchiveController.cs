using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lexicon_LMS.Models;

namespace Lexicon_LMS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ArchiveController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Archive
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Teacher).Where(c => c.EndDate < DateTime.Now);
            return View(courses.ToList());
        }

        // GET: Archive/Details/5
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
                

        

        // GET: Archive/Edit/5
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

        // POST: Archive/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Archive/Delete/5
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

        // POST: Archive/Delete/5
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
