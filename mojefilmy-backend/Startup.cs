using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using mojefilmy_backend.Data;
using System;

namespace mojefilmy_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MojeFilmyContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), 
                new MySqlServerVersion(new Version(8, 0, 21))));

            services.AddControllers();

            // Konfiguracja Swaggera
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MojeFilmy API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080") // Adres frontendu
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MojeFilmy API V1");
                c.RoutePrefix = string.Empty; // Ustawia Swagger UI na ścieżce głównej aplikacji
            });

            app.UseRouting();
            app.UseCors(); // Użyj CORS przed UseRouting

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
