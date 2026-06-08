using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TanitaTracker.Infrastructure.Data;
using TanitaTracker.Infrastructure.Services;
using TanitaTracker.Infrastructure.Repositories;
using TanitaTracker.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

// TODO: Dependency Injections when done
builder.Services.AddScoped<IScanRepository, ScanRepository>();
builder.Services.AddScoped<IOcrService, AzureOcrService>();

// TODO: Add ApplicationDBContext when done
builder.Services.AddDbContext<ApplicationDbContext>();


// CORS configuration policy so Blazor WebAssembly can communicate with this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7000") // TODO: Change to production url
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Documentation tooling for testing:
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp"); // Link the Policy to the App

// Add Authenticaion and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map the Identity endpoints (Login, Register, etc.)
app.MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();

