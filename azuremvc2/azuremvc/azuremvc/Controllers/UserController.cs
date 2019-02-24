using azuremvc.Models;
using StorageAccountLibrary;
using StorageAccountLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace azuremvc.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("UserProfile")]
        public ActionResult UserProfile()
        {
            var u = new User();
            u.Name = "Poonam";
            u.ContactNo = "12345678";

            return View(u);
        }

        [HttpPost]
        [ActionName("UserProfile")]
        public ActionResult UserProfilePost(User user)
        {
            string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
            string extension = Path.GetExtension(user.ImageFile.FileName);

            fileName = fileName + extension;
            string relativeDirectoryPath = ConfigurationManager.AppSettings["ImageDirectoryPath"].ToString();

            string directoryPath = HttpContext.Server.MapPath(relativeDirectoryPath);

            string imageFullPath = directoryPath + "\\" + fileName;

            user.ImageFile.SaveAs(imageFullPath);

            ViewBag.message = "File uploaded successfully";
            return View(user);
        }


        [HttpGet]
        [ActionName("UserProfileBlob")]
        public ActionResult UserProfileBlob()
        {
            var u = new User();
            u.Name = "Poonam";
            u.ContactNo = "12345678";

            return View(u);
        }


        [HttpPost]
        [ActionName("UserProfileBlob")]
        public ActionResult UserProfileBlobPost(User user)
        {
            string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
            string extension = Path.GetExtension(user.ImageFile.FileName);
            //Path.GetFullPath(user.ImageFile.)

            fileName = fileName + extension;
            string relativeDirectoryPath = ConfigurationManager.AppSettings["ImageDirectoryPath"].ToString();

            string directoryPath = HttpContext.Server.MapPath(relativeDirectoryPath);

            string imageFullPath = directoryPath + "\\" + fileName;

            user.ImageFile.SaveAs(imageFullPath);

            //first get entire filepath
            //not working
            //string path = HttpContext.Server.MapPath(user.ImageFile.FileName);

            //library call
            //Storage.SetupStorage();
            //Storage.UploadToBlobStorage(imageFullPath);
            bool isUploaded = Models.User.UploadFilesToContainer(imageFullPath, fileName,"poonam");
            if(isUploaded)
            {
                ViewBag.message = "File uploaded successfully";

            }
            return View(user);
        }


        [HttpGet]
        [ActionName("UserProfileBlobList")]
        public ActionResult UserProfileBlobList()
        {
            List<BlobData> list = Models.User.ListContainerBlobStorage();

            return View(list);
        }

        [HttpPost]
        public ActionResult DownloadFile(string filePath)
        {
            // string blobPath = Path.GetFullPath(filePath);

            //return new FileStreamResult(new FileStream(new Uri(filePath).AbsolutePath,FileMode.Open),"image/jpeg");
            //string azureFilePath = filePath;
            string relativeDirectoryPath = ConfigurationManager.AppSettings["ImageDirectoryPath"].ToString();

            string directoryPath = HttpContext.Server.MapPath(relativeDirectoryPath);
            //string directoryPath = @"C:\Users\Poonam\Downloads\";


            string imageFullPath = directoryPath + "\\" + filePath.Substring(filePath.LastIndexOf('/') + 1);

          //  Models.User.DownloadImageFromAzure(filePath, imageFullPath, "poonam");

            using (WebClient client = new WebClient())
            {
                //fileName = fileName + extension;

                client.DownloadFile(filePath, imageFullPath);
            }

            FileStream fs = new FileStream(imageFullPath, FileMode.Open);
            byte[] buf = new byte[fs.Length];
            int br = fs.Read(buf, 0, buf.Length);
            string contentType = MimeMapping.GetMimeMapping(imageFullPath);

            // Response.AppendHeader("Content-Disposition", "attachment; filename=" + filePath.Substring(filePath.LastIndexOf('/') + 1));
            //Response.AddHeader("Content-Length", fs.Length.ToString());
            //Response.ContentType = "application/octet-stream";
            // Response.OutputStream.Write(buf, 0, buf.Length);

            //return new FileStreamResult(new FileStream(imageFullPath,FileMode.Open),"image/jpeg");
            //return  File(buf, "application/octet-stream");

            //return File(buf, System.Net.Mime.MediaTypeNames.Application.Octet);
            //return new FileStreamResult(fs, "application/octet-stream");


            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filePath.Substring(filePath.LastIndexOf('/') + 1),
                Inline = false,
                //DispositionType = System.Net.Mime.ContentDisposition.DispositionType.
            };
            //fs.Close();
            //Response.AppendHeader("Content-Disposition", cd.ToString());

            //Response.Headers.Remove("Content-Disposition");
            //Response.Headers.Add("Content-Disposition", "attachment;filename="+ filePath.Substring(filePath.LastIndexOf('/') + 1));
           // Response.AppendHeader("Content-Disposition", "attachment;filename="+ filePath.Substring(filePath.LastIndexOf('/') + 1));

            //Response.Buffer = true;
         //return File(buf, System.Net.Mime.MediaTypeNames.Application.Octet, filePath.Substring(filePath.LastIndexOf('/') + 1));


          //  return new FileContentResult(buf, contentType);
            return new FileStreamResult(fs, cd.ToString());

            //return null;
        }



    }
}