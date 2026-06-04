using ACE_PC.BlazorServer.Components;
using ACE_PC.BlazorServer.Middleware;
using ACE_PC.BlazorServer.Providers;
using ACE_PC.BlazorServer.Services;
using ACE_PC.BlazorServer.UseCases.Auth;
using ACE_PC.BlazorServer.UseCases.Categories;
using ACE_PC.BlazorServer.UseCases.Comments;
using ACE_PC.BlazorServer.UseCases.Like;
using ACE_PC.BlazorServer.UseCases.Quotes;
using ACE_PC.BlazorServer.UseCases.Users;
using Blazored.Toast;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using Sotsera.Blazor.Toaster;
using Sotsera.Blazor.Toaster.Core.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


//Blazor Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]!))
    };
});

// HttpClient
builder.Services.AddHttpClient("client",options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("baseUri")!);
});


//services
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomeAuthenticationProvider>();

builder.Services.AddMudServices();

builder.Services.AddScoped<CategoriesUseCase>();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<AuthUseCase>();
builder.Services.AddScoped<UserUseCases>();
builder.Services.AddScoped<QuoteUseCases>();
builder.Services.AddScoped<CommentUseCases>();
builder.Services.AddScoped<CommonUserInterfaceService>();
builder.Services.AddScoped<LikesUseCase>();




//APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
