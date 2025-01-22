using AzureIoTServer.Models;
using AzureIoTServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddBearerToken();
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<IoTDBContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<IoTDBContext>();
    

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "MQTT Service";
});
builder.Services.AddHostedService<MQTTService>();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Azure IoT Server",
        Version = "v1"
    });
});

var app = builder.Build();

using(var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<IoTDBContext>();
    dbContext.Database.Migrate();

}

// With Azure, Swagger always need to be generated
app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
