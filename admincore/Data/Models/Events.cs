﻿using System;
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
        //[ForeignKey("ImageDocumentId")]
        //public virtual Document ImageDocument { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        public EventPriority Priority { get; set; }

        public string Description { get; set; }

        public EventStatus Status { get; set; }
    }
}
