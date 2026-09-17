using Microsoft.EntityFrameworkCore;

namespace BooksGAD.Models;

public partial class BookManagerContext : DbContext
{
    public BookManagerContext(DbContextOptions<BookManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("Books_pkey");

            entity.HasIndex(e => e.FtsAnnotation, "fts_gin_idx").HasMethod("gin");

            entity.HasIndex(e => e.Name, "unq_name").IsUnique();

            entity.Property(e => e.Authors).HasMaxLength(120);
            entity.Property(e => e.FtsAnnotation).HasComputedColumnSql("to_tsvector('russian'::regconfig, \"Annotation\")", true);
            entity.Property(e => e.Name).HasMaxLength(80);
        });
        
    }
}
