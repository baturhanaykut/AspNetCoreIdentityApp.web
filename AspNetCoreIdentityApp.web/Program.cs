using AspNetCoreIdentityApp.web.ClaimProvider;
using AspNetCoreIdentityApp.web.Extensions;
using AspNetCoreIdentityApp.web.Models;
using AspNetCoreIdentityApp.web.OptionsModel;
using AspNetCoreIdentityApp.web.PermissionsRoot;
using AspNetCoreIdentityApp.web.Requirements;
using AspNetCoreIdentityApp.web.Seeds;
using AspNetCoreIdentityApp.web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});


builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentityWithExt();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExechangeExpireRequrimentHandeler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequrimentHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ÝstanbulPolicy", policy =>
    {
        policy.RequireClaim("city", "Ýstanbul", "Erzincan");
    });

    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExechangeExpireRequriment());
    }); 
    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequriment() { ThreshOldAge =18});
    }); 
    options.AddPolicy("OrderPermissionReadOrDeletePolicy", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Read);
        policy.RequireClaim("permission", Permission.Order.Delete);
        policy.RequireClaim("permission", Permission.Stock.Delete);
    });

});

builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilerder = new CookieBuilder
    {
        Name = "UdemyAppCookie"
    };
    options.LoginPath = new PathString("/Home/Signin");
    options.LogoutPath = new PathString("/Member/logout");
    options.AccessDeniedPath = new PathString("/Member/AccessDenied");

    options.Cookie = cookieBuilerder;
    options.ExpireTimeSpan = TimeSpan.FromDays(60);
    options.SlidingExpiration = true;

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleManager);
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
