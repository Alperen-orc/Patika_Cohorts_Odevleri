using Microsoft.EntityFrameworkCore;
using Patika_Hafta1_Odev.Extension;
using Patika_Hafta1_Odev.Handler;
using Patika_Hafta1_Odev.Models;
using Patika_Hafta1_Odev.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Swagger implementasyonu
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IProductService,FakeProductService>();

var app = builder.Build();

//GlobalExceptionMiddleware kullanılması
app.UseGlobalExceptionMiddleware();
//Loglama için ReguestResponseLogging kullanılması
app.UseRequestResponseLogging();

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
