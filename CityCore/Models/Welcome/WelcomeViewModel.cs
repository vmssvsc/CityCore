using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCore.Models
{
    public class WelcomeViewModel
    {
        public List<SliderImage> SliderImages { get; set; }
        
        public List<SmartProjectViewModel> SmartProject { get; set; }
    }

    public class Project
    {

    }

    public class SliderImage
    {
        public int SequenceNumber { get; set; }
        public string Url { get; set; }
    }


}
