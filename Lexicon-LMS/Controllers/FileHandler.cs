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

namespace Lexicon_LMS.Controllers
{
    public class FileHandler : Controller
    {

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
    }

   
}