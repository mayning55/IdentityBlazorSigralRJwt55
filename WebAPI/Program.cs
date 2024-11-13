using ClassLibrary.Services;
using ClassLibrary.Settings;
using Microsoft.Net.Http.Headers;

using Microsoft.AspNetCore.ResponseCompression;
using WebAPI.Hubs;//5

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

builder.Services.AddHostedService<ClassInit>();//初始化管理员Admin信息
builder.Services.AddScoped<InitAdmin>();

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
    });//!
}

app.UseHttpsRedirection();

app.UseAuthentication();//身份验证,在CORS跨域后
app.UseAuthorization();//授权,在身份验证后

app.MapControllers();

app.MapHub<ChatHub>("/chathub");//5

app.Run();
