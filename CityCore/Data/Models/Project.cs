using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CityCore.Common.Enums;

namespace CityCore.Data.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LocalityDetails { get; set; }

        public string BoundaryEast { get; set; }

        public string BoundaryWest { get; set; }

        public string BoundaryNorth { get; set; }

        public string BoundarySouth { get; set; }

        public DateTime Date { get; set; }

        public ProjectStatus Status { get; set; }

        public int ImageDocumentId { get; set; }
        [ForeignKey("ImageDocumentId")]
        public virtual Document ImageDocument { get; set; }

        //public ICollection<ProjectInitiative> ProjectInitiatives { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class ProjectInitiative
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Initiative { get; set; }

        //public int ProjectId { get; set; }
        //[ForeignKey("ProjectId")]
        //public virtual Project Project { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class SmartCityProject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        public SmartCityProjectDisplayLocation DisplayLocation { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class ABDProjectDetail
    {
        [Key]
        public int Id { get; set; }

        public int NoOfProjects { get; set; }

        public float Cost { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }



}
