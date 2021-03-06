﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using admincore.Models;
using admincore.Data.Models;

namespace admincore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumDocumentMap> AlbumDocumentMaps { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectInitiative> ProjectInitiatives { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<SmartCityProject> SmartCityProjects { get; set; }
        public DbSet<ABDProjectDetail> ABDProjectDetails { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsDocumentMap> NewsDocumentMaps { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Tender> Tenders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
