using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCore.Models
{
    public class CareerViewModel
    {
        public int Id { get; set; }

        public String PRONo { get; set; }
       
        public string Department { get; set; }
     
        public string PostName { get; set; }

        public DateTime StarDate { get; set; }
       
        public DateTime EndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PostDocName { get; set; }

        public string FromDocName { get; set; }
        public string FormDocURL { get; set; }
        public string PostDocURL { get; set; }
    }
}
