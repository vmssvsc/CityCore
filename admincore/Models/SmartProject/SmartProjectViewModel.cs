using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace admincore.Models.SmartProject
{
    public class SmartProjectViewModel
    {

        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
        public int ImgId { get; set; }

        public string Url { get; set; }

        public Common.Enums.SmartCityProjectDisplayLocation? DisplayLocation { get; set; }

    }

    public class SmartProjectListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DisplayLocation { get; set; }
    }
}
