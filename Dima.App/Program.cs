using Dima.App;
using Dima.App.Handlers;
using Dima.App.Security;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddScoped<CookieAuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped<ICookieAuthenticationStateProvider>(provider => provider.GetRequiredService<CookieAuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services
    .AddHttpClient(name: Configuration.HttpClientName, configureClient: opt =>
        {
            opt.BaseAddress = new Uri(Configuration.BackendUrl);
        })
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddTransient<IAccountHandler, AccountHandler>();

await builder.Build().RunAsync();
