using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<PersonManagerContext>(op => op.UseInMemoryDatabase("PersonManager"));

        // Register services, repositories.and validators
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IValidator<PersonViewModel>, PersonRequestValidator>();

        // Add memory cache (used by the DepartmentService for caching).
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Create database so the data seeds
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using var context = serviceScope.ServiceProvider.GetRequiredService<PersonManagerContext>();
            context.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}