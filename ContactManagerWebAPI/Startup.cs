using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ContactManagerWebAPI.Data;
using ContactManagerWebAPI.Interfaces;
using ContactManagerWebAPI.Services;
using System;

namespace ContactManagerWebAPI
{
    public class Startup
    {
        readonly string AllowOrigin = "my allow origin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowOrigin,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                                  });

            });

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.IgnoreNullValues = false;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddDbContext<ContactManagerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ContactManagerDbConnectionString"))//,
                    //sqlServerOptionsAction: sqlOptions =>
                    //{
                    //    sqlOptions.EnableRetryOnFailure(
                    //    maxRetryCount: 5,
                    //    maxRetryDelay: TimeSpan.FromSeconds(30),
                    //    errorNumbersToAdd: null);
                    //}), ServiceLifetime.Transient
            );

            services.AddTransient<IContactManagerService, ContactManagerService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactManagerWebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactManagerWebAPI v1"));

                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                MockData.Seed(serviceScope.ServiceProvider);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowOrigin);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
