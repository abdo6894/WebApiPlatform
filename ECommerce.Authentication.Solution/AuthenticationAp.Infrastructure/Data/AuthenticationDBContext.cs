using AuthenticationApi.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAp.Infrastructure.Data
{
    public class AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> option) : DbContext(option)
    {
        public DbSet<AppUser> Users { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        //    modelBuilder.Entity<AppUser>().Property(u => u.Name).IsRequired();
        //    modelBuilder.Entity<AppUser>().Property(u => u.TelephoneNumber).IsRequired();
        //    modelBuilder.Entity<AppUser>().Property(u => u.Address).IsRequired();
        //    modelBuilder.Entity<AppUser>().Property(u => u.Email).IsRequired().HasMaxLength(256);
        //    modelBuilder.Entity<AppUser>().Property(u => u.Password).IsRequired();
        //    modelBuilder.Entity<AppUser>().Property(u => u.Role).IsRequired();
        //}
    }
}
