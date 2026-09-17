using BooksGAD.Models;
using BooksGAD.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services.AddDbContextPool<BookManagerContext>(options => options.UseNpgsql(dbConnectionString));

builder.Services.AddScoped<IBookManagerRepository, BookManagerRepository>();

builder.Services.AddHttpLogging(logging => { });
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.MapControllers();

app.MapGet("/api/health", () => "Hello World!");

app.Run();