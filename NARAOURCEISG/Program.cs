using Microsoft.EntityFrameworkCore;
using NARAOURCEISG.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NARAOUCREISGDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NARAOUCREISGDBContext") ?? throw new InvalidOperationException("Connection string 'NARAOUCREISGDBContext' not found.")));

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NARAOURCEISGDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

var app = builder.Build();

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
