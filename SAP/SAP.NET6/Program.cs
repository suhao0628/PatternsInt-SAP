using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAP.NET6;
using SAP.NET6.Data;
using SAP.NET6.Services;
using SAP.NET6.Services.Catalogue;
using SAP.NET6.Services.Catalogue.Implementations;
using SAP.NET6.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
builder.Services.AddSingleton(mapConfig.CreateMapper());
builder.Services.AddScoped<ICatalogueDataProvider, CatalogueDataProvider>();
builder.Services.AddScoped<ICatalogueAdministration, CatalogueAdministration>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ApplicationDbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //Automigration of database
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    //Seed database
    var initializer = scope.ServiceProvider.GetService<ApplicationDbInitializer>();
    initializer.SeedUsers();
    initializer.SeedCatalogue();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
