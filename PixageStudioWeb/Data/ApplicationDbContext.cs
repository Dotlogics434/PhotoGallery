using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<ImagePool> ImagePools { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Category> Categories { get; set; }



    }
}
