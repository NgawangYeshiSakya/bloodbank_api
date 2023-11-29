using BBank.Data;
using BBank.Repositories.Contracts;
using BBank.Repositories;
using Microsoft.EntityFrameworkCore;
using BBank;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setting up Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClinicLocationRepository, ClinicLocationRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthMiddleware();//Custom auth

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
