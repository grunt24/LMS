using BCASLibrary.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCASLibrary.Models
{
    public class AppDbContext : IdentityDbContext<LibraryUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, GenreName = "Romance" },
                new Genre { Id = 2, GenreName = "Comedy" },
                new Genre { Id = 3, GenreName = "SciFy" }
                );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Hunter x Hunter", Description = "Testing Description", Isbn = "1er23ftr", Author = "Prince", GenreId = 1, ImageUrl = "" }
                );

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LibraryUserEntityConfiguration());

        }

    }
}
public class LibraryUserEntityConfiguration : IEntityTypeConfiguration<LibraryUser>
{
    public void Configure(EntityTypeBuilder<LibraryUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(225);
        builder.Property(u => u.LastName).HasMaxLength(225);

    }
}
