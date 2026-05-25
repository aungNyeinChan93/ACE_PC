using ACE_PC.Api.Extensions;
using ACE_PC.BL.Services;
using ACE_PC.Database.Data;
using ACE_PC.Domain.Interfaces.Auth;
using ACE_PC.Domain.Interfaces.Categories;
using ACE_PC.Domain.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.MapAuth(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("client",p=> p.WithOrigins("https://localhost:7046").AllowAnyHeader().AllowAnyMethod());
});

//APP
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
