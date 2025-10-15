using Applicazione_1.ExtentionMethods;
using Applicazione_1.Middleware;
using Applicazione_1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<JWT>();
builder.Services.AddScoped<EmailValidation>();
builder.Services.AddOpenApi();
builder.Services.ConfigureApp(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "VeniceCom.com",
            ValidAudience = "VeniceCom.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AhmedMahmoudAbdelkarimAhmedEbrahimKhaleefa123456789"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
