using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Interfaces;
using NotesApp.DataAccess;
using NotesApp.Services.Services;
using NotesApp.Services.Interfaces;
using NotesApp.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dbcontext
builder.Services.AddDbContext<NotesDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(NotesDbContext).Assembly.FullName)));

builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddScoped<INotesRepository, NotesRepository>();

//Add automapper
builder.Services.AddAutoMapper(
    typeof(NotesService).Assembly
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
