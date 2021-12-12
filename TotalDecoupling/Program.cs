using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TotalDecoupling.BusinessLayer.Services;
using TotalDecoupling.BusinessLayer.Services.Interfaces;
using TotalDecoupling.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TotalDecoupling", Version = "v1" });
});

builder.Services.AddDbContext<IDbContext, DataContext>(options =>
{
    options.UseInMemoryDatabase("MyDatabase");
});

builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TotalDecoupling v1"));
}

app.MapControllers();

app.Run();
