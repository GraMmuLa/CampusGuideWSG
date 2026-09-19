using CampusGuideWSG.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString!))
);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
