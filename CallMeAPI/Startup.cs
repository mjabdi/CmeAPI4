using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CallMeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Stripe;

namespace CallMeAPI
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

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddDbContext<MyDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CallmeDB")));



            services.UseJWT(Configuration["Jwt:Issuer"], Configuration["Jwt:Issuer"], Configuration["Jwt:Key"]);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    

            services.ConfigureCors();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });

            //        services.AddMvc()
            //.AddJsonOptions(
            //    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);


            if (Program.onAzure)
            {
                app.UseHsts();
            }

           // app.UseHttpsRedirection();
           


            app.UseAuthentication();
            app.UseMvc();
        }
    }


    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
