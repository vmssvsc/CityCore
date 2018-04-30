using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models
{
    public class TeamMemberViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        
        [MaxLength(50)]
        public string Post { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        public int ImgId { get; set; }

        public string ImageUrl { get; set; }
    }


    public class TeamMemberListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Post { get; set; }
    }


}
