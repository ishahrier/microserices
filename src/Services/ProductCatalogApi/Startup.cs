using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;

namespace ProductCatalogApi {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.Configure<CatalogSettings> (Configuration);
            services.AddDbContext<CatalogContext> (o => o.UseSqlServer (Configuration["ConnectionString"]));
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(Options=>{
                Options.DescribeAllEnumsAsStrings();
                Options.SwaggerDoc("v1",new Swashbuckle.AspNetCore.Swagger.Info{
                    Title = "Shoes on container - Product Catalog HTTP API",
                    Version = "v1",
                    Description = "The product catalog microservices HTTP API.",
                    TermsOfService = "It is free and open source"
                });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            using (var scope = app.ApplicationServices.CreateScope ()) {
                var ctx = scope.ServiceProvider.GetService<CatalogContext> ();
                CatalogSeed.SeedAsync (ctx).Wait ();
            }
            app.UseSwagger()
            .UseSwaggerUI(
                c=>c.SwaggerEndpoint($"/swagger/v1/swagger.json","ProductCatalogAPI V1")
            );
            //app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}