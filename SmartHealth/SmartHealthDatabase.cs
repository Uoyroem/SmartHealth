using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SmartHealth
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }

    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int SpecialityId { get; set; }
        public Speciality Speciality { get; set; } = null!;
        public int WorkExperience { get; set; }
        public string AcademicDegree { get; set; } = null!;
        public string Photo { get; set; } = null!;
    }

    public class SmartHealthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public SmartHealthDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SmartHealthDatabase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
            modelBuilder.Entity<Speciality>().HasIndex(speciality => speciality.Name).IsUnique();
        }
    }
}
