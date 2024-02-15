using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PigeonDLCore.Models;
using File = PigeonDLCore.Models.File;

namespace PigeonDLCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FolderShared> FoldersShared { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Folders
            modelBuilder.Entity<Folder>()
                .HasIndex(e => e.URL)
                .IsUnique();

            modelBuilder.Entity<Folder>()
                .Property(e => e.DateUploaded)
                .HasDefaultValueSql("getdate()");

            //FoldersShared
            modelBuilder.Entity<FolderShared>()
                .Property(e => e.DateAdded)
                .HasDefaultValueSql("getdate()");


            //Files
            modelBuilder.Entity<File>()
                .Property(e => e.Downloads)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<File>()
                .Property(e => e.DateUploaded)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<File>()
                .HasIndex(e => e.URL)
                .IsUnique();
        }
    }
}