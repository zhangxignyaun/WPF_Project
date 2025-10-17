using DailyAPI.AutoMappers;
using DailyAPI.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(m =>
{
    string path = AppContext.BaseDirectory + "DailyAPI.XML";
    m.IncludeXmlComments(path,true);
});
//注入数据库上下文
builder.Services.AddDbContext<DailyDBContext>(m =>m.UseSqlServer(builder.Configuration.GetConnectionString("Connstr")));
//AutoMapper
builder.Services.AddAutoMapper(cfg => { },typeof(AutoMapperSetting));

builder.Logging.AddLog4Net("Log4net.config");//添加log4net支持
builder.Logging.AddFilter("System", LogLevel.Warning); // 过滤System日志（可选）
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // 过滤Microsoft日志（可选）

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
