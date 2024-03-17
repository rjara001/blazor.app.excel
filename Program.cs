using BlazorAppExcel;
using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;
using BlazorAppExcel.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Register named HttpClient instance with specific configuration
builder.Services.AddHttpClient("MyNamedClient", client =>
{
    var url = builder.Configuration["ServerUrl:BaseAddress"];
    client.BaseAddress = new Uri(url);
    client.Timeout = TimeSpan.FromSeconds(60000);
    // You can configure other properties of HttpClient here if needed
});

builder.Services.AddFluentUIComponents();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<IExcelService, ExcelService>();
builder.Services.AddSingleton<ISessionSingletonService, SessionSingletonService>();


await builder.Build().RunAsync();
