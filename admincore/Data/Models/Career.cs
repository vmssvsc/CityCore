using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Data.Models
{
    public class Career
    {

        [Key]
        public int Id { get; set; }

        public String PRONo { get; set; }

        public String Department { get; set; }
        public String PostName { get; set; }

        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        

        

       

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
