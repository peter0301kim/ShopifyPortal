using BrainzParentsPortal.Data;
using BrainzParentsPortal.Helpers;
using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Shared;
using BrainzParentsPortal.Shared.SettingModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Newtonsoft.Json;
using Paytec.Integration.Shopify;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddScoped<ProtectedSessionStorage>();

//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());


builder.Services.AddSingleton<PortalDbConnectionSettings>(
    new PortalDbConnectionSettings(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile)
    );

builder.Services.AddSingleton<ShopifySettings>(
    new ShopifySettings(GlobalSettings.Instance.ShopifySettingsPathFile)
    );

builder.Services.AddSingleton<EmailServerSettings>(
    new EmailServerSettings(GlobalSettings.Instance.EmailServerSettingsPathFile)
    );

builder.Services.AddSingleton<BrainzParentsPortalSettings>(
    new BrainzParentsPortalSettings(GlobalSettings.Instance.BrainzParentsPortalSettingsPathFile)
    );

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices(options => { options.PopoverOptions.CheckForPopoverProvider = false; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();