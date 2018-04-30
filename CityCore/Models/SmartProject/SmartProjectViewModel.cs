using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCore.Models
{
    public class SmartProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public Common.Enums.SmartCityProjectDisplayLocation DisplayLocation { get; set; }
    }
}
