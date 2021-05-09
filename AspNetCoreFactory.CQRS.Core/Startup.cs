using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AspNetCoreFactory.CQRS.Core
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private IWebHostEnvironment _env { get; }

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ICache, Cache>();
            services.AddScoped<ILookup, Lookup>();
            services.AddScoped<IRollup, Rollup>();
            services.AddScoped<IEvent, Event>();

            services.AddHttpContextAccessor();

            // Create connectionstring with root path location
            var connectionString = _config.GetConnectionString("CQRS")
                                          .Replace("{Path}", _env.ContentRootPath);
            services.AddDbContext<CQRSContext>(options =>
                                           options.UseSqlServer(connectionString));

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .AddRazorRuntimeCompilation()
                .AddFlatAreas(new FlatAreaOptions());

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              CQRSContext db, IHttpContextAccessor httpContextAccessor)
        {
            ServiceLocator.Register(httpContextAccessor);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            db.Database.ExecuteSqlRaw("SELECT 1"); // Warmup localdb for better users experience.
        }
    }
}
