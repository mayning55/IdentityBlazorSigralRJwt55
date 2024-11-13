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



builder.Services.DIServices(builder.Configuration);//!ע�����

builder.Services.AddHostedService<ClassInit>();//��ʼ������ԱAdmin��Ϣ
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

app.UseAuthentication();//�����֤,��CORS�����
app.UseAuthorization();//��Ȩ,�������֤��

app.MapControllers();

app.MapHub<ChatHub>("/chathub");//5

app.Run();
