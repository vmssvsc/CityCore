using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Data.Models
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Post { get; set; }

        public int ImageDocumentId { get; set; }

        [ForeignKey("ImageDocumentId")]
        public virtual Document ImageDocument { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
