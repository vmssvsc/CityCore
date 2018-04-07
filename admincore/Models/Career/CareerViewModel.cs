using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Models.Career
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


        public string PostDocName { get; set; }


        [Required]
        public DateTime StarDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


          public string DocName { get; set; }

        
        



    }
}
