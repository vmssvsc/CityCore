using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models
{
    
    public class TenderViewModel
    {
        public int Id { get; set; }
        [Required]
        public string TenderDesc { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public int? File1Id { get; set; }
        public int? File2Id { get; set; }
        public int FormDocId { get; set; }
        public int PostDocId { get; set; }


        public string FormDocName { get; set; }
        public string PostDocName { get; set; }
        public string File1Name { get; set; }
        public string File2Name { get; set; }

        [Required]
        public IFormFile PostDocument { get; set; }
        [Required]
        public IFormFile FormDocument { get; set; }
        [Required]
        public IFormFile File1 { get; set; }
        [Required]
        public IFormFile File2 { get; set; }

        
        public string FormDocURL { get; set; }
        public string PostDocURL { get; set; }
        public string File1URL { get; set; }
        public string File2URL { get; set; }
    }

    public class TenderListViewModel
    {
        //"Id", "ProNo", "Department", "Post", "StartDate", "EndDate",
        public int Id { get; set; }
                       
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public object TenderDesc { get; internal set; }
    }
}
