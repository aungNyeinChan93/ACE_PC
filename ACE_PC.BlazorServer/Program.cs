using ACE_PC.BlazorServer.Components;
using ACE_PC.BlazorServer.Providers;
using ACE_PC.BlazorServer.UseCases.Auth;
using ACE_PC.BlazorServer.UseCases.Categories;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Sotsera.Blazor.Toaster;
using Sotsera.Blazor.Toaster.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// 
builder.Services.AddHttpClient("client",options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("baseUri")!);
});

//services
builder.Services.AddScoped<CategoriesUseCase>();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<AuthUseCase>();


//Auth
builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider , CustomeAuthenticationProvider>();



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
