using ChatRoomAPI;
using ChatRoomAPI.Data;
using ChatRoomAPI.Middleware;
using ChatRoomAPI.Models;
using ChatRoomAPI.Repositories;
using ChatRoomAPI.Services;
using ChatRoomWeb.Data;
using ChatRoomWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var cryptoSettingsSection = builder.Configuration.GetSection("CryptoSettings");
builder.Services.Configure<CryptoSettings>(cryptoSettingsSection);

var adminSettingsSection = builder.Configuration.GetSection("AdminSettings");
builder.Services.Configure<AdminSettings>(adminSettingsSection);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost:7158",
            ValidAudience = "localhost:7158",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cryptoSettingsSection["JwtSigningKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override(Constants.Microsoft, Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Debug()
    .WriteTo.File("SignUpLog", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//var dbSettingsSection = builder.Configuration.GetSection("DatabaseSettings");
//builder.Services.Configure<Dat>
// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<ITokenRepository, TokenRepository>();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseApiKey();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.Run();
