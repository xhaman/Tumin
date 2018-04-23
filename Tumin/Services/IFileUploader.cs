using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Services
{
    public interface IFileUploader
    {
        Task<string> UploadImage(IFormFile image, string extension);

        Task<string> UploadFile(IFormFile image);

    }
}
