﻿using Serilog;

var builder = WebApplication.CreateBuilder(args);


//Seqi Confiqurasiya etmek
// Serilog konfiqurasiyası
Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq("http://localhost:5341") // Seq serveri URL-i
    .CreateLogger();

// Serilog-u logging sisteminə əlavə etmək
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

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
