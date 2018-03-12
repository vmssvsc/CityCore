using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }
   
        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }

    public class EventListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }
    }
}
