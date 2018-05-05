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
            AlbumPhoto,
            SmartProjectImage,
            CareerFile,
            CareerForm,
            NewsImage,
            TeamMemberImage
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

        public enum NewsType
        {
            City,
            General
        }


        public enum NewsStatus
        {
            Active,
            Inactive
        }

        public enum NewsPriority
        {
            Low,
            Medium,
            High
        }


        public enum ProjectStatus
        {
            Upcoming,
            Ongoing,
            Completed
        }

        public enum SmartCityProjectDisplayLocation
        {
            Home,
            ProjectsPage
        }

        public enum InitiativeType
        {
            PANCity,
            ABD
        }
    }
}
