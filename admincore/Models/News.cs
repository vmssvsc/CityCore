using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static admincore.Common.Enums;

namespace admincore.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public NewsPriority Priority { get; set; }

        public NewsType NewsType { get; set; }

        [Required]
        public string Description { get; set; }

        public NewsStatus Status { get; set; }

        public List<IFormFile> Images { get; set; }
    }

    public class NewsListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public NewsPriority Priority { get; set; }

        public int NoOfImages { get; set; }

        public NewsStatus Status { get; set; }

        public NewsType NewsType { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
