using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => 
{
    cors.AddPolicy("CORSPolicy", config =>
    {
        config
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("https://localhost:7220");
    });
});

// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=./Data/Service.db"));
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddScoped<IMessageBusWrapper, MessageBusWrapper>();
builder.Services.AddHostedService<RabbitMqListener>();

builder.Services.AddScoped<IRabbitMqScopedService, RabbitMqScopedService>();
builder.Services.AddCron<RabbitMqBackgroundService>(x => 
{
    x.TimeZone = TimeZoneInfo.Local;
    x.CronExpression = @"*/1 * * * *";
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Service",
        Version = "v1",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
