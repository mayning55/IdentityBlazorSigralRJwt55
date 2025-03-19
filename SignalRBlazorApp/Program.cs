using Blazored.LocalStorage;
using DateClassLibrary;
using DateClassLibrary.Data;
using LoginClassLibrary.Login;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SignalRBlazorApp;
using SignalRBlazorApp.ServiceClass;
using SignalRBlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddTransient<CustomHttpHandler>();//!!
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7286");
}).AddHttpMessageHandler<CustomHttpHandler>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7286") });

builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<IUser, AccountAuth>();
builder.Services.AddScoped<IClientDataInterface<Author>, ClientImplementation<Author>>();//注册接口和实现方法
builder.Services.AddScoped<IClientDataInterface<Book>, ClientImplementation<Book>>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();//!!

await builder.Build().RunAsync();
