using NpgsqlTypes;

namespace BooksGAD.Models;

public partial class Book
{
    public long BookId { get; set; }

    public string Authors { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly PublishingDate { get; set; }

    public string? Annotation { get; set; }

    public NpgsqlTsVector? FtsAnnotation { get; set; }
}
