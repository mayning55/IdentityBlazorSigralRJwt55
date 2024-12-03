using ClassLibrary.Services;
using ClassLibrary.Settings;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.ResponseCompression;
using WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();//5

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});//5



builder.Services.DIServices(builder.Configuration);//!注册服务

builder.Services.AddHostedService<ClassInit>();//注册托管服务，初始化Admin信息
builder.Services.AddScoped<InitAdmin>();//初始化Admin用户，完成后可删除。

var app = builder.Build();

app.UseResponseCompression();//5

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("https://localhost:7185")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });//跨域请求：参阅：https://learn.microsoft.com/zh-cn/aspnet/core/security/cors?view=aspnetcore-8.0
}

app.UseHttpsRedirection();

app.UseAuthentication();//身份验证，在CORS后，
app.UseAuthorization();//授权，在身份验证后。

app.MapControllers();

app.MapHub<ChatHub>("/chathub");//5

app.Run();
