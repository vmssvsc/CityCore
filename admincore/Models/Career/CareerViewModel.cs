using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models
{
    public class CareerViewModel
    {
        public int Id { get; set; }
        public String PRONo { get; set; }
        [Required]
        [MaxLength(50)]
        public string Department { get; set; }
        [Required]
        [MaxLength(200)]
        public string PostName { get; set; }
       
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string FormDocName { get; set; }
        public string PostDocName { get; set; }
        public IFormFile PostDocument { get; set; }
        public IFormFile FormDocument { get; set; }
        public int FormDocId { get; set; }
        public int PostDocId { get; set; }
    }

    public class CareerListViewModel
    {
        //"Id", "ProNo", "Department", "Post", "StartDate", "EndDate",
        public int Id { get; set; }

        public string ProNo { get; set; }

        public string Department { get; set; }

        public string Post { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
