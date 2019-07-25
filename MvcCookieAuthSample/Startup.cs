using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcCookieAuthSample.Services;
using Microsoft.EntityFrameworkCore;
using MvcCookieAuthSample.Models;
using IdentityServer4.Services;
using System.Reflection;

namespace MvcCookieAuthSample
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResource())
                //.AddTestUsers(Config.GetTestUsers());
                //.AddConfigurationStore(options=> {
                //    options.ConfigureDbContext = builder => {
                //        builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                //            sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                //    };
                //})
                //.AddOperationalStore(options => {
                //    options.ConfigureDbContext = builder =>
                //    {
                //        builder.UseSqlServer(
                //            Configuration.GetConnectionString("DefaultConnection"),
                //            sql =>
                //            {
                //                sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                //            });
                //    };
                //})
                .AddAspNetIdentity<ApplicationUser>()
                .Services.AddScoped<IProfileService, ProfileService>();
            // //Add-Migration init -Context ConfigurationDbContext -OutputDir Data/Migrations//IdentityServer/ConfigurationDb


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //密码规则设置
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
            });

            services.AddScoped<ConsentService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
