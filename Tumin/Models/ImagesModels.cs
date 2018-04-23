using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Models
{
    public class ImagesModels
    {
    }

    public class AvatarImage
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsCurrentAvatar { get; set; }
    }
}
