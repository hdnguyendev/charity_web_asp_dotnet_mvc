using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DonationsWeb.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DonationsWebContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DonationsWebContext") ?? throw new InvalidOperationException("Connection string 'DonationsWebContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();



//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<DonationsWebContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddControllers();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
//// Tạo và khởi tạo vai trò
//await InitializeDatabase(app);

app.UseRouting();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//// Phương thức để khởi tạo vai trò
//async Task InitializeDatabase(WebApplication app)
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        await SeedRoles(roleManager);
//    }
//}

//async Task SeedRoles(RoleManager<IdentityRole> roleManager)
//{
//    string[] roleNames = { "Admin", "Donor" };

//    foreach (var roleName in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(roleName))
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }
//}