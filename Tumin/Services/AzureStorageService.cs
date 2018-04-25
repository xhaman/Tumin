using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Services
{
    public class AzureStorageService:IFileUploader
    {
        public IConfiguration Configuration { get; }
        public AzureStorageService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json");
            Configuration = builder.Build();

        }

        public Task<string> UploadFile(IFormFile image)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadImage(IFormFile image, string extension)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration["ConnectionStrings:AzureConnectionString"]);

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("botfiles");
                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }

                string prefixlogo = Guid.NewGuid().ToString() + extension;
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(prefixlogo);
                await blockBlob.UploadFromStreamAsync(image.OpenReadStream());
                ///   blockBlob.Properties.ContentType = "image/jpeg";
                await blockBlob.SetPropertiesAsync();

                string imageUrl = blockBlob.Uri.ToString();

                return imageUrl;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

    }
}
