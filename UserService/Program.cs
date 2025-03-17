using Serilog;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<EventBusClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5033"); // EventBus API-nin URL-i
});

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
