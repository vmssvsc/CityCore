using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static admincore.Common.Enums;

namespace admincore.Data.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; } 

        public string Title { get; set; }

        public DateTime Date { get; set; }
      
        public NewsPriority Priority { get; set; }

        public NewsType NewsType { get; set; }

        public string Description { get; set; }

        public NewsStatus Status { get; set; }

        public ICollection<NewsDocumentMap> NewsDocumentMaps { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class NewsDocumentMap
    {
        [Key]
        public int Id { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        public int NewsId { get; set; }
        [ForeignKey("NewsId")]
        public virtual News News { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

