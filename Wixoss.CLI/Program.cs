using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wixoss.CLI.Data;
using Wixoss.CLI.Tests;
using Wixoss.Models.Translations;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Services.AddLocalization();

builder.Services.AddTransient<WixossPrimitive>();

builder.Services.AddTransient<DataCommand>();
builder.Services.AddTransient<ScrubCommand>();
builder.Services.AddTransient<UniqueCommand>();
builder.Services.AddTransient<TestCommand>();
builder.Services.AddTransient<I18nCommand>();

using var host = builder.Build();
await host.StartAsync();

var services = host.Services;
var lifetime = services.GetRequiredService<IHostApplicationLifetime>();

var ret = await new RootCommand("Wixoss.CLI maintaince tool"){
                    services.GetRequiredService<TestCommand>(),
                    services.GetRequiredService<DataCommand>(),
                }.InvokeAsync(args);

lifetime.StopApplication();
await host.WaitForShutdownAsync();

return ret;
