using Microsoft.EntityFrameworkCore;
using NotificationService.NotificationDb;

var builder = WebApplication.CreateBuilder(args);

//Memori db quraq 

builder.Services.AddDbContext<NotificationDbContext>(options => options.UseInMemoryDatabase("NotificationDb"));



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
