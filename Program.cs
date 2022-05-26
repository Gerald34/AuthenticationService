using AuthenticationService.Services;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.DbContexts;
using AuthenticationService.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthenticationService.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

// Database contexts
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("UserDatabase"));
});

builder.Services.AddDbContext<AccountStatusDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("UserDatabase"));
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Bearer",
//         policy => policy.Requirements.Add(new Authorization.AuthorizeAttribute()));
// });

// Services
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddSingleton<RSAIDNumberService, RSAIDNumberService>();
builder.Services.AddSingleton<JwtService, JwtService>();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

app.Run();
