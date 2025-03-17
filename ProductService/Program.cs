using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ProductService.Services.Implementation;
using ProductService.Services.Infrastructure;
using ProductService.Services.Middlewares;
using Serilog;
using AutoMapper;
using ProductService.Infrastructure.Repositories.Implementation;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.UnitOfWork;
using ProductService.Infrastructure.Repositories.Interfaces;
using EventBusService.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRabbitMqService>(sp =>
    new RabbitMqService());


// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // "ApiGateway"
            ValidAudience = builder.Configuration["Jwt:Audience"], // "OrderAndProductServices"
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


//Db-connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("DefaultConnection"));

builder.Services.AddAutoMapper(typeof(Program)); // Add AutoMapper here
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductServiceLayer, ProductServiceLayer>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryServiceLayer, CategoryServiceLayer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//Seq Configuration
static IHostBuilder CreateHostBuilder(string[] args) =>
   Host.CreateDefaultBuilder(args)
       .ConfigureAppConfiguration((context, config) =>
       {
           // Additional Configuration files (optional)
       })
       .ConfigureLogging((context, logging) =>
       {
           logging.ClearProviders();
           logging.AddSerilog(); // Add serilog
       })
       .ConfigureServices((context, services) =>
       {
           // Serilog Configuration
           Log.Logger = new LoggerConfiguration()
               .WriteTo.Seq("http://localhost:5341") // Seq server URL
               .WriteTo.Console() // (optional)
               .CreateLogger();

           services.AddSingleton(Log.Logger); 
       })
.ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder.UseStartup<Program>(); // Use Program class as Startup
       });


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Custom Exception Middleware-i ?lav? edirik
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
