using Blazored.LocalStorage;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SignalRBlazorApp;
using SignalRBlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7286");
}).AddHttpMessageHandler<CustomHttpHandler>();//ͨ�� DI ע��CustomHttpHandler�����ͷ����
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7286") });

builder.Services.AddScoped<GetHttpClient>();
//builder.Services.AddScoped<ILocalStorageService,ILocalStorageService>();
builder.Services.AddScoped<IUser, AccountAuth>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();//!!

await builder.Build().RunAsync();
