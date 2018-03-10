using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static admincore.Common.Enums;

namespace admincore.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public int Title { get; set; }

        public DateTime EventDate { get; set; }

        public int ImageDocumentId { get; set; }
        [ForeignKey("ImageDocumentId")]
        public virtual Document ImageDocument { get; set; }

        public int FileDocumentId { get; set; }
        [ForeignKey("FileDocumentId")]
        public virtual Document FileDocument { get; set; }

        public EventPriority Priority { get; set; }
    }
}
