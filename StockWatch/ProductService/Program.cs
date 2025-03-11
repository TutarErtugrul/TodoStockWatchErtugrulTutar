using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductServiceAPI.Hubs;
using ProductServiceAPI.Repositories;
using ProductServiceAPI.Services;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Service API", Version = "v1" });
});

// JWT Authentication
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
// Redis Bağlantısı
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect($"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}"));


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRedisPublisherService, RedisPublisherService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<RabbitMQPublisher>();
builder.Services.AddScoped<RabbitMQConsumer>();
var emailService = new EmailService();
var consumer = new RabbitMQConsumer(emailService);
Task.Run(() => consumer.StartListening());
// CORS ayarları
var corsPolicy = "AllowSpecificOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:8080") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service API v1");
    });
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<StockHub>("/stockHub").RequireCors("AllowSpecificOrigin");
});

app.Run();
