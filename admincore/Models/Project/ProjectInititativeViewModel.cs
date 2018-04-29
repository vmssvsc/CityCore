using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static admincore.Common.Enums;

namespace admincore.Models.Project
{
    public class ProjectInititativeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }


        [Required]
        [MaxLength(200)]
        public string Initiative { get; set; }

        public InitiativeType InitiativeType { get; set; }


    }

    public class ProjectInititativeListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Initiative { get; set; }


    }
}
