using Microsoft.EntityFrameworkCore;
using GBC_TRAVEL_GROUP_88.Data;
using Microsoft.AspNetCore.Identity;
using GBC_TRAVEL_GROUP_88.Services;
using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Custom Error Page
    app.UseStatusCodePagesWithRedirects("/Home/NotFound?statusCode={0}");
   
}
else
{
    app.UseDeveloperExceptionPage();
}

using var scope = app.Services.CreateScope();
var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

try
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

	// automatically run migrations
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();


}
catch (Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occurred seeding the roles for the system");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Flights}/{action=Index}/{id?}");
    

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
