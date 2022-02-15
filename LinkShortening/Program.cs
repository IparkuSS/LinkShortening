using LinkShortening.Services;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings;
using LinkShortening.Settings.Contracts;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.Configure<LinkShorteningDatabaseSettings>(
    configuration.GetSection(nameof(LinkShorteningDatabaseSettings)));

builder.Services.AddSingleton<ILinkShorteningDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<LinkShorteningDatabaseSettings>>().Value);

builder.Services.AddScoped<ILinksService, LinksService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
