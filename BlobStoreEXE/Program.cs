using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;

namespace BlobStoreEXE
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["AzureBlobStorage"].ConnectionString;
            var source = ConfigurationManager.AppSettings["source"];
            var dest = ConfigurationManager.AppSettings["dest"];

            CloudStorageAccount acc = CloudStorageAccount.Parse(connectionstring);
            CloudBlobClient client = acc.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(dest);
            container.CreateIfNotExists();
            var path = @"C:\Temp\EncodingTime.csv";
            var dt = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss");
            var key = dt + "-" + path;
            CloudBlockBlob blob = container.GetBlockBlobReference(key); 
            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                blob.UploadFromStream(fs);
            }
        }
    }
}
