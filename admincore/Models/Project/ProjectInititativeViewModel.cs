using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace admincore.Models.Project
{
    public class ProjectInititativeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }


        [Required]
        [MaxLength(200)]
        public string Initiative { get; set; }


    }
}
