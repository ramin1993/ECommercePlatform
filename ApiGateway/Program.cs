using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

//Seqi Configuration
static IHostBuilder CreateHostBuilder(string[] args) =>
   Host.CreateDefaultBuilder(args)
       .ConfigureAppConfiguration((context, config) =>
       {
           // ?lav? konfiqurasiya fayllar? ?lav? ed? bil?rsiniz
       })
       .ConfigureLogging((context, logging) =>
       {
           logging.ClearProviders();
           logging.AddSerilog(); // Serilog-u ?lav? et
       })
       .ConfigureServices((context, services) =>
       {
           // Serilog konfiqurasiyas?
           Log.Logger = new LoggerConfiguration()
               .WriteTo.Seq("http://localhost:5341") // Seq serverinin URL-si (localda Seq serverini qurmu?uqsa, yerli adresi istifad? et)
               .WriteTo.Console() // Konsol loglama (iste?e ba?l?)
               .CreateLogger();

           // H?r hans? bir ?lav? servisl?r ?lav? edin
           services.AddSingleton(Log.Logger); // Logger-i DI konteynerin? ?lav? et
       })
.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Program>(); // Use Program class as Startup
});
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Ocelot
builder.Services.AddOcelot();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
