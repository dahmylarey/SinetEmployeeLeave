using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SinetEmployeeLeave.Data;
using SinetEmployeeLeave.Implementation;
using SinetEmployeeLeave.Models;
using SinetEmployeeLeave.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LeaveManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<LeaveManagementDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Add Role and identity
//builder.Services.AddIdentity<Employee, IdentityRole>()
//    .AddEntityFrameworkStores<LeaveManagementDbContext>()
//    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Register Repositories 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<LeaveRequestRepository>();

var app = builder.Build();

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

// Enable Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages(); // For Identity UI


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
