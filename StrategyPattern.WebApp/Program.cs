using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StrategyPattern.WebApp.DataAccess;
using StrategyPattern.WebApp.Models;
using StrategyPattern.WebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductRepository>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

    var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == Settings.ClaimDatabaseType)?.Value;

    var context = serviceProvider.GetRequiredService<ApplicationContext>();

    if (claim == null)
        return new ProductRepositoryFromSqlServer(context);

    var databaseType = (DatabaseType)int.Parse(claim);

    return databaseType switch
    {
        DatabaseType.SqlServer => new ProductRepositoryFromSqlServer(context),
        DatabaseType.MongoDb => new ProductRepositoryFromMongoDb(builder.Configuration),
        _ => throw new NotImplementedException()
    };
});

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSql"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationContext>()
  .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
