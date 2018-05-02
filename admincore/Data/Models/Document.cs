using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static admincore.Common.Enums;

namespace admincore.Data.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public string URL { get; set; }

        public string FileName { get; set; }

        public DocumentCategory? DocumentCategory { get; set; }

        public string DocumentContentType { get; set; }

        public ICollection<SliderImage> SliderImages { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<AlbumDocumentMap> AlbumDocumentMap { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<SmartCityProject> SmartCityProjects { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
