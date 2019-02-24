using StorageAccountLibrary;
using StorageAccountLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace azuremvc.Models
{
    public class User
    {
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayAttribute(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [Display(Name = "Upload File")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public static bool UploadFilesToContainer(string filepath, string destinationFilename, string containerName)
        {
           // Storage.SetupStorage();
            Storage.UploadToBlobStorage(filepath, destinationFilename,containerName);
            return true;
        }

        public static List<BlobData> ListContainerBlobStorage()
        {
            // Storage.SetupStorage();
            List<BlobData> blobs = Storage.ListContainerBlobStorage("poonam");
            return blobs;
        }

        public static void DownloadImageFromAzure(string filepath, string destinationFilePath, string containerName)
        {
            Storage.DownloadFromBlobStorage( filepath,  destinationFilePath,  containerName);
        }

    }
}