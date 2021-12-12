using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TotalDecoupling.BusinessLayer.Services;
using TotalDecoupling.DataAccessLayer;

namespace TotalDecoupling;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TotalDecoupling", Version = "v1" });
        });

        services.AddDbContext<IDbContext, DataContext>(options =>
        {
            options.UseInMemoryDatabase("MyDatabase");
        });

        services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<IImageService, ImageService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TotalDecoupling v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
