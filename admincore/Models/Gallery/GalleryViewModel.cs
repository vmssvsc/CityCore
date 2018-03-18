using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models.Gallery
{
    public class GalleryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

       
        public List <IFormFile> Files{ get; set; }
    }
}
