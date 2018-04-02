using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CityCore.Common.Enums;

namespace CityCore.Data.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
