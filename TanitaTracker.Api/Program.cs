using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Instead of Swagger we use OpenApi https://aka.ms/aspnet/openapi
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>();

// TODO: Dependency Injections when done
// builder.Services.AddScoped<IScanRepository, ScanRepository>();
// builder.Services.AddScoped<IOcrService, AzureOcrService>();

// TODO: Add ApplicationDBContext when done
builder.Services.AddIdentityApiEndpoints<IdentityUser>();
// .AddEntityFrameworkStores<ApplicationDbContext>();

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
    app.MapOpenApi();
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

