using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace admincore.Models
{
    public class VideoViewModel
    {

          public int Id { get; set; }

         [Required] 
         [MaxLength(50)]
         public string Title { get; set; }


        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public string VideoUrl { get; set; }

          
        
    }




    public class VideoListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        
    }




















}
