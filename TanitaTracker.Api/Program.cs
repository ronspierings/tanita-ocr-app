using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>();

// TODO: Dependency Injections when done
// TODO: Add ApplicationDBContext when done

// CORS configuration policy so Blazor WebAssembly can communicate with this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7000")
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
app.UseCors("AllowBlazorApp"); // Link the policy to the App

// Add Authenticaion and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map the Identity endpoints (Login, Register, etc.)
app.MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();

