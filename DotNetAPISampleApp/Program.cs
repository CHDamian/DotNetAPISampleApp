using DotNetAPISampleApp.Data;
using DotNetAPISampleApp.Interfaces.IRepository.ExaminationInterfaces;
using DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Interfaces.IService.ExaminationInterfaces;
using DotNetAPISampleApp.Interfaces.IService.IdentityInterfaces;
using DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces;
using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Repositories.ExaminationRepositories;
using DotNetAPISampleApp.Repositories.IdentityRepositories;
using DotNetAPISampleApp.Repositories.ResearchRepositeries;
using DotNetAPISampleApp.Sevices.ExaminationServices;
using DotNetAPISampleApp.Sevices.IdentityServices;
using DotNetAPISampleApp.Sevices.ResearchSevices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=research.db"));

// Add services to the container.
builder.Services.AddScoped<IResearchRepository, ResearchRepository>();
builder.Services.AddScoped<IResearchService, ResearchService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddScoped<IResearchSignedRepository, ResearchSignedRepository>();
builder.Services.AddScoped<ISigningService, SigningService>();

builder.Services.AddScoped<IExaminationRepository, ExaminationRepository>();
builder.Services.AddScoped<IExaminationService, ExaminationService>();


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    var roleNames = new[] { "Patient", "Researcher", "Administrator" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminUser = await userManager.FindByNameAsync("Admin");
    if (adminUser == null)
    {
        adminUser = new User
        {
            Email = "admin@admin.com",
            UserName = "Admin",
            PESEL = "01010012345",
            Name = "AdminName",
            Surname = "AdminSurname"
        };
        var result = await userManager.CreateAsync(adminUser, "Password123$"); // SET NEW PASSWORD!

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
