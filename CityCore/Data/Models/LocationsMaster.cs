using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityCore.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<State> States { get; set; }
    }

    public class State
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country {get; set;}

        public ICollection<City> Cities { get; set; }
    }

    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State States { get; set; }
    }
}
