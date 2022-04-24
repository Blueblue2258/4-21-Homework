// 1. 安裝 Nuget 套件 Microsoft.EntityFrameworkCore.SqlServer
// 2. 安裝 Nuget 套件 Microsoft.EntityFrameworkCore.Tools
// 3. 產生 DB model：
//      Scaffold-DbContext "Server=DESKTOP-6IN097U\SQLEXPRESS;Initial Catalog=Homework;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
// 4. 刪除 DbContext 中寫死的連線字串
// 5. 在 appsettings.Development.json 增加連線字串
// 6. 註冊 Entity Framework service



using _4._21Homework.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HomeworkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
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
