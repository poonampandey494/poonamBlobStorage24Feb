using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageAccountLibrary.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountLibrary
{
    public class Storage
    {
        static string connectionString = @"DefaultEndpointsProtocol = https; AccountName=poonamvm01diag;AccountKey=O+j2fjcxVxE0PG03j1FfUqKuezaeuICj/2FCfuXX7b5BYgfDzOoCywTTJz/OdrXHzmwR2UcrXA6bfsdou7ieDA==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount = null;

        public static void SetupStorage()
        {
            cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=storagepoonamgpv;AccountKey=dAVIbft7P3SASYAzB6BZXDaQfFc/C4n12xwlzivHiv8A1u+raEJ/E8qom46nAs9RjgWFzh9GzZ6FsUg/u7Scwg==;EndpointSuffix=core.windows.net");


        }

        public static bool UploadToBlobStorage(string filepath,string destinationFilename, string containerName)
        {
            try
            {
            if (cloudStorageAccount == null)
            {
                SetupStorage();
            }

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            // CloudBlobContainer container = cloudBlobClient.GetContainerReference("poonam");
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            container.CreateIfNotExistsAsync().Wait();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(destinationFilename);

            //using (FileStream fs = new FileStream(@"E:\Study Material\Azure\VM\azuremvc2\azuremvc2\azuremvc\imageIcon.png", FileMode.OpenOrCreate))
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))

            {
                blockBlob.UploadFromStream(fs);
            }
            return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
               
            }
        }

        public static List<BlobData> ListContainerBlobStorage(string containerName)
        {
            try
            {
                if (cloudStorageAccount == null)
                {
                    SetupStorage();
                }
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                // CloudBlobContainer container = cloudBlobClient.GetContainerReference("poonam");
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

                if (container.Exists())
                {
                   IEnumerable<IListBlobItem> blobList = container.ListBlobs();
                   List<BlobData> blobDataList = new List<BlobData>();
                    foreach (CloudBlockBlob item in blobList)
                    {
                        BlobData blob = new BlobData() { name = item.Name, path = item.Uri };
                        blobDataList.Add(blob);
                        
                    }
                    return blobDataList;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }


        public static bool DownloadFromBlobStorage(string filepath, string destinationFilePath, string containerName)
        {
            try
            {
                if (cloudStorageAccount == null)
                {
                    SetupStorage();
                }

                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                // CloudBlobContainer container = cloudBlobClient.GetContainerReference("poonam");
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

                container.CreateIfNotExistsAsync().Wait();

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filepath);

                //using (FileStream fs = new FileStream(@"E:\Study Material\Azure\VM\azuremvc2\azuremvc2\azuremvc\imageIcon.png", FileMode.OpenOrCreate))
                //using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))

                //{
                //    blockBlob.DownloadToStream(fs);
                //}

                using (var fileStream = System.IO.File.OpenWrite(destinationFilePath))
                {
                    blockBlob.DownloadToStream(fileStream);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }



    }
}
