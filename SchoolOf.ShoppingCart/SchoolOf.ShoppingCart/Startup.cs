using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SchoolOf.Common;
using SchoolOf.Data;
using SchoolOf.Data.Abstraction;
using SchoolOf.Mappers;
using SchoolOf.ShoppingCart.Filters;

namespace SchoolOf.ShoppingCart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolOf.ShoppingCart", Version = "v1" });
            });
            // database container registration
            services.AddScoped<DatabaseContext>();

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Global Filter Exception
            services.AddScoped<GlobalExceptionFilter>();

            // AutoMapper
            services.AddAutoMapper(typeof(ProductsMapperProfile).Assembly);

            services.AddControllers(opt => {
                opt.Filters.AddService<GlobalExceptionFilter>();
            });

            // configuration (options)
            services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolOf.ShoppingCart v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
