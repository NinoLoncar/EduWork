using EduWork.Data;
using EduWork.WebApi;
using EduWork.WebApi.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(Program));


var swaggerAdConfiguration = new SwaggerAuthorizationConfiguration();
builder.Configuration.GetSection(SwaggerAuthorizationConfiguration.Section).Bind(swaggerAdConfiguration);
builder.Services.Configure<SwaggerAuthorizationConfiguration>(builder.Configuration.GetSection(SwaggerAuthorizationConfiguration.Section));


builder.Services.AddConfiguredSwagger(swaggerAdConfiguration);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
    {
        builder.WithOrigins("https://localhost:7110", "http://localhost:5143")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(swaggerAdConfiguration.ClientId);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowBlazorClient");


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
