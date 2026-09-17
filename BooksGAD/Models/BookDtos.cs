namespace BooksGAD.Models;

public record BookCreatingFormDto(string Name, string Authors, string Annotation);

public record BookFromDbDto(string Name, string Authors, string Annotation, DateOnly PublishingDate);