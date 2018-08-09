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
    public class FileHandler : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Document UploadFile (HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var originalFilename = Path.GetFileName(upload.FileName);
                //var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                string fileId = Guid.NewGuid().ToString().Replace("-", "");

                var fileName = fileId + "_" + originalFilename;

                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"));
                var save = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"), fileName);
                upload.SaveAs(save);

                var file = new Document
                {
                    FileName = fileName,
                    DisplayName = originalFilename,
                    UploadDate = DateTime.Now,
                    //CourseID = course.ID,
                    Filepath = path,
                    //User = user
                };

                return file;
            }

            return null;
        }

        public Document UploadFile(HttpPostedFileBase upload, Activity activity)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var originalFilename = Path.GetFileName(upload.FileName);
                var user = db.Users.Find(System.Web.HttpContext.Current.User.Identity.GetUserId());
                Models.Module tgtModule = db.Modules.Find(activity.ModuleID);
                Activity tgtActivity = db.Activities.Find(activity.ID);


                string fileId = Guid.NewGuid().ToString().Replace("-", "");

                var fileName = fileId + "_" + originalFilename;

                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"),tgtModule.Course.CourseCode,tgtModule.ModuleTitle,tgtActivity.Name);
                var save = Path.Combine(path, fileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                upload.SaveAs(save);
                

                var file = new Document
                {
                    FileName = fileName,
                    DisplayName = originalFilename,
                    UploadDate = DateTime.Now,
                    ActivityID = activity.ID,
                    Filepath = path,
                    User = user
                };
                tgtActivity.Documents.Add(file);
                db.SaveChanges();

                return file;
            }

            return null;
        }

        public Document UploadFile(HttpPostedFileBase upload, Assignment assignment)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var originalFilename = Path.GetFileName(upload.FileName);
                var user = db.Users.Find(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var teacher = db.Users.Find(user.UserCourse.TeacherID);
                
                Models.Module tgtModule = db.Modules.Find(assignment.ModuleID);
                Assignment tgtAssignment = db.Activities.OfType<Assignment>().Where(a => a.ID == assignment.ID).FirstOrDefault();


                string fileId = Guid.NewGuid().ToString().Replace("-", "");

                var fileName = fileId + "_" + originalFilename;

                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"), tgtModule.Course.CourseCode, tgtModule.ModuleTitle, tgtAssignment.Name);
                var save = Path.Combine(path, fileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                upload.SaveAs(save);

                var file = new Document
                {
                    FileName = fileName,
                    DisplayName = originalFilename,
                    UploadDate = DateTime.Now,
                    ActivityID = assignment.ID,
                    Filepath = path,
                    UserAssignment = true,
                    User = user
                };
                tgtAssignment.Documents.Add(file);
                var alert = NewActivityAlert(assignment, user, file);

                teacher.Notifications.Add(alert);
                db.SaveChanges();

                return file;
            }

            return null;
        }

        public Document UploadFile(HttpPostedFileBase upload, Course course)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var originalFilename = Path.GetFileName(upload.FileName);
                var user = db.Users.Find(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var tgtCourse = db.Courses.Find(course.ID);

                string fileId = Guid.NewGuid().ToString().Replace("-", "");

                var fileName = fileId + "_" + originalFilename;

                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"),course.CourseCode);
                var save = Path.Combine(path, fileName);

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                upload.SaveAs(save);

                var file = new Document
                {
                    FileName = fileName,
                    DisplayName = originalFilename,
                    UploadDate = DateTime.Now,
                    CourseID = course.ID,
                    Filepath = path,
                    User = user
                };
                tgtCourse.Documents.Add(file);
                db.SaveChanges();

                return file;
            }

            return null;
        }

        public FileContentResult DownloadFile (string filePath, string fileName)
        {
            string fullName = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, filePath, fileName);

            if (!System.IO.File.Exists(fullName))
            {
                return null;
            }

            string contentType = MimeMapping.GetMimeMapping(filePath);
            byte[] fileBytes = GetFile(fullName);

            fileName = fileName.Substring(fileName.IndexOf("_"));

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = false
            };

            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", cd.ToString());
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

        public Notification NewActivityAlert(Assignment assignment, ApplicationUser user, Document doc)
        {
            var tgtModule = db.Modules.Find(assignment.ModuleID);
            ApplicationUser recip = db.Users.Find(doc.User.UserCourse.TeacherID);

            var notif = new Notification();
            notif.Subject = user.FullName + " has submitted an assignment";
            notif.Body = user.FullName + " has submitted an assignment to \"" + assignment.Name + "\" activity in" + tgtModule.ModuleTitle + "(" + tgtModule.Course.CourseName + "(" + tgtModule.Course.CourseCode + "))";
            notif.Attachment = doc;
            notif.Sender = user;
            notif.Recipients = new List<ApplicationUser> { recip };
            notif.RecipientID = recip.Id;
            notif.DateSent = DateTime.Now;


            return notif;
        }
    }

   
}