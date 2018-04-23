using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Services
{
    public class AzureStorageService
    {

        public async Task<string> UploadFileToContainer(IFormFile image, string extension)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=encourer;AccountKey=hCgPKIyj3zMdMuW7ttkNPna0KybAXyAd3qWjG7cbJihmHGDrjNuUbX2zcAJLRo6jckyAzM09VZEw/LvtZzCbWA==;EndpointSuffix=core.windows.net");

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
            catch (Exception)
            {

                return null;
            }
        }

    }
}
