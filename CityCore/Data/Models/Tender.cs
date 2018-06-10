using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using static CityCore.Common.Enums;

namespace CityCore.Data.Models
{
    public class Tender
    {
        [Key]
        public int Id { get; set; }

        public String TenderDesc { get; set; }

        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }

        public TenderStatus TenderStatus { get; set; }

        public int? DocumentId { get; set; }

        public int? FormDocumentId { get; set; }

        public int? File1Id { get; set; }

        public int? File2Id { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}