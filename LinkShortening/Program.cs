using LinkShortening.Extensions;
using LoggerService.Contracts;
using NLog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMongoContext(configuration);
builder.Services.ConfigureServices();

builder.Services.AddControllers();
builder.Services.ConfigureAuthentication(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var resolvedLoggerManager = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(resolvedLoggerManager);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
