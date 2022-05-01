using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Interfaces;
using NotesApp.DataAccess;
using NotesApp.Services.Services;
using NotesApp.Services.Interfaces;
using NotesApp.DataAccess.Repositories;
using NotesApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NotesApp.Services.Dto;
using NotesApp.Services.Dto.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dbcontext
builder.Services.AddDbContext<NotesDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(NotesDbContext).Assembly.FullName)),
    ServiceLifetime.Transient,
    ServiceLifetime.Transient);

builder.Services.AddTransient<NotesSeeder>();
builder.Services.AddTransient<INotesService, NotesService>();
builder.Services.AddTransient<IUsersService, UsersService>();

builder.Services.AddTransient<INotesRepository, NotesRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

//Validators
builder.Services.AddTransient<IValidator<CreateUserDto>, CreateUserValidator>();
ValidatorOptions.Global.LanguageManager.Enabled = false;

//Add automapper
builder.Services.AddAutoMapper(
    typeof(NotesService).Assembly
);

var app = builder.Build();
await SeedDatabase();

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

async Task SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<NotesSeeder>();
        await dbInitializer.Seed();
    }
}