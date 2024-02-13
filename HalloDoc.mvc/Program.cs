//using halloDoc.Models;
//using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using BusinessLogic.Services;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext"))); 
builder.Services.AddScoped<ILoginService,LoginService>();
builder.Services.AddScoped<IPatientService, PatientService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Register
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
