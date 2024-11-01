using ProductSalesAPI.Application.ApiUtilities;
using ProductSalesAPI.Application.ApiUtilities.Middleware;
using ProductSalesAPI.Presentation.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.InitializeDatabaseAsync();

app.MapHub<NotificationsHub>("/notificationHub");

app.Run();
