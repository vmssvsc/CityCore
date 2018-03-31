using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace admincore.Common
{
    public class Enums
    {
        public enum SettingsValues
        {
            NoOfSliderImages = 1
        }

        public enum DocumentCategory
        {
            SliderImage,
            ProjectImage,
            UserProfileImage,
            ProjectVideo,
            EventFile,
            EventImage,
            AlbumPhoto
        }

        public enum DocumentType
        {
            Image,
            File,
            Media
        }

        public enum EventPriority
        {
            Low,
            Medium,
            High
        }

        public enum EventStatus
        {
            Upcoming,
            Cancelled,
            Completed
        }
    }
}
