using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CityCore.Common.Enums;

namespace CityCore.Models
{
    public class WelcomeViewModel
    {
        public List<SliderImage> SliderImages { get; set; }
        
        public List<SmartProjectViewModel> SmartProject { get; set; }

        public List<NewsViewModel> News { get; set; }
    }

    public class NewsViewModel
    {
        public int Id { get; set; }

        public string CoverURL { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public NewsType NewsType { get; set; }

        public NewsPriority NewsPriority { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class SliderImage
    {
        public int SequenceNumber { get; set; }
        public string Url { get; set; }
    }

    public class NewsItemViewModel
    {
        public string Title { get; set; }

        public string URL { get; set; }
    }
}
