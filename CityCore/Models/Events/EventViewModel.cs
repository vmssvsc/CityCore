﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCore.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string File { get; set; }
    }


    public class AlbumViewModel
    {
        public int Id { get; set; }
        
        public string Albumname { get; set; }

        public string CoverImage { get; set; }

        public DateTime CreatedOn { get; set; }

        public int NoOfPhotos { get; set; }
    }

    public class PhotoViewModel
    {
        public int Id { get; set; }

        public string Photoname { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }


    }


    public class MediaViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string VideoUrl { get; set; }

    }





















}
