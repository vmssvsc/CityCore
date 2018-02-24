using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CityCore.Common.Enums;

namespace CityCore.Data.Models
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

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
