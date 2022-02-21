using System.Text;
using LinkShortening.Extensions;
using LinkShortening.Services;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings;
using LinkShortening.Settings.Contracts;
using LoggerService;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.





LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.Configure<LinkShorteningDatabaseSettings>(
    configuration.GetSection(nameof(LinkShorteningDatabaseSettings)));

builder.Services.AddSingleton<ILinkShorteningDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<LinkShorteningDatabaseSettings>>().Value);


builder.Services.AddScoped<IUrlServices, UrlServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.ConfigureLoggerService();

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.Configure<LinkShorteningDatabaseSettings>(
    builder.Configuration.GetSection("LinkShortening"));


builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<UrlServices>();//ddd

builder.Services.AddControllers();// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtKey").ToString())),
            ValidateIssuer = false,
            ValidateAudience = false

        };
    });




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//ILoggerManager logger = app;
// Configure the HTTP request pipeline.
var resolvedLoggerManager = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(resolvedLoggerManager);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();





app.Run();
