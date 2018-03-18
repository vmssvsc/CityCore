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

        public string Title { get; set; }

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

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class Album
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AlbumDocumentMap> AlbumDocumentMaps { get; set; }
    }

    public class AlbumDocumentMap
    {
        [Key]
        public int Id { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        public int AlbumsId { get; set; }   
        [ForeignKey("AlbumsId")]
        public virtual Album Albums { get; set; }
    }   
}
