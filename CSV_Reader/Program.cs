using CSV_Reader.Data;
using CSV_Reader.Repositories;
using CSV_Reader.Repositories.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CSV_Reader;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<CsvPersonDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
        });
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddControllersWithViews();
        var app = builder.Build();
        var supportedCultures = new[] { new CultureInfo("uk-UA"), new CultureInfo("en-US") };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("uk-UA"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        app.UseRequestLocalization(localizationOptions);


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Person}/{action=Index}/{id?}");

        app.Run();
    }
}
