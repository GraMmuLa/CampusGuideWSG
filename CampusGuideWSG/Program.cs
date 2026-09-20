using CampusGuideWSG.Context;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;
using CampusGuideWSG.Services;
using CampusGuideWSG.Services.Implementations;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Helpers.Implementations;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString!))
);

#region Adding Repositories
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IEntranceRepository, EntranceRepository>();
builder.Services.AddScoped<IModeratorRepository, ModeratorRepository>();
builder.Services.AddScoped<IModeratorBuildingRepository, ModeratorBuildingRepository>();
#endregion

#region Adding Services
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEntranceService, EntranceService>();
builder.Services.AddScoped<IModeratorService, ModeratorService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region Adding Helpers
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

builder.Services.AddOpenApi();

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}

app.Run();
