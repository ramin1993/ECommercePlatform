using Microsoft.EntityFrameworkCore;
using PaymentService.PaymentDB;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Seqi Confiqurasiya etmek
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
               .CreateLogger();

           // H?r hans? bir ?lav? servisl?r ?lav? edin
           services.AddSingleton(Log.Logger); // Logger-i DI konteynerin? ?lav? et
       })
.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Program>(); // Use Program class as Startup
});


//Memory db quraq
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseInMemoryDatabase("ProductDB"));

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
