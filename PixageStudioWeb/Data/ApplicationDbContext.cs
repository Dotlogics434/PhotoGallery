using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<RoleViewModel> roleViewModels { get; set; }
        public DbSet<UserRoleViewModel> userRoleViewModels{ get; set; }
        public DbSet<ImagePool> ImagePool { get; set; }
        public DbSet<Content> Content { get; set; }



    }
}
