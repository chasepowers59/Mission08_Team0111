using Mission08_Team0111.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Mission08_Team0111.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// register repositories BEFORE calling Build()
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Database and repository
builder.Services.AddDbContext<TaskContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// Apply pending migrations and create database if needed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // helpful dev page while developing
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// serve wwwroot static files (css/js/bootstrap)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// If you have custom static assets mapping extension methods, call them AFTER UseRouting if required.
// If not, remove MapStaticAssets / WithStaticAssets lines below.
app.MapStaticAssets(); // keep only if this extension exists in your project

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// If your MapControllerRoute returned something that needs WithStaticAssets(), only keep that variant
// .WithStaticAssets();

app.Run();