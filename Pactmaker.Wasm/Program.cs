using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using Pactmaker.Shared;
using Pactmaker.Shared.Shared;
using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<WixossCardList>();
builder.Services.AddScoped<WixossDeckList>();
builder.Services.AddMudServices(c =>
{
    c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    c.SnackbarConfiguration.PreventDuplicates = false;
    c.SnackbarConfiguration.VisibleStateDuration = 2000;
    c.SnackbarConfiguration.ShowTransitionDuration = 200;
    c.SnackbarConfiguration.HideTransitionDuration = 200;
});

var cardlist = builder.Services.BuildServiceProvider().GetRequiredService<WixossCardList>();
await cardlist.PopulateAsync();

var jso = new JsonSerializerOptions();
jso.Converters.Add(new WixossCardJsonConverter(cardlist));

builder.Services.AddBlazoredLocalStorage(c =>
{
    c.JsonSerializerOptions = jso;
});

await builder.Build().RunAsync();
