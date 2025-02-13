using IoTFallServer.BackgroundServices.MQTT;
using IoTFallServer.Models;
using IoTFallServer.Services.Helpers;
using IoTFallServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using IoTFallServer.Options;

var builder = WebApplication.CreateBuilder(args);

// Adding SecretsKey vault
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IoTDBContext>();

builder.Services.AddAuthentication(schemes =>
{
    schemes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    schemes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    schemes.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<IoTDBContext>();

builder.Services.AddScoped<IHMMDService, HMMDService>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<User>()
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/manage/info" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/manage/2fa" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/register" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/resendConfirmationEmail" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/forgotPassword" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/resetPassword" ? Results.BadRequest() : await next(context))
    .AddEndpointFilter(async (context, next) => context.HttpContext.Request.Path == "/confirmEmail" ? Results.BadRequest() : await next(context));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
