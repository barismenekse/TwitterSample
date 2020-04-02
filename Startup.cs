using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twitter.Context;
using Twitter.Entity;
using Twitter.Repository;

namespace Twitter
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<UnitOfWork, UnitOfWork>();
            services.AddDbContext<TwitterContext>(
                options => options.UseSqlServer(@"server=.;database=TwitterDB;Trusted_Connection=True;").UseLazyLoadingProxies());

            services.AddDbContext<IdDbContext>(
               options => options.UseSqlServer(@"server=.;database=IdentityDb;Trusted_Connection=True;;MultipleActiveResultSets=true;").UseLazyLoadingProxies());

            services.AddScoped(typeof(Repository<Following>), typeof(Repository<Following>));
            services.AddScoped(typeof(Repository<Like>), typeof(Repository<Like>));
            services.AddScoped(typeof(Repository<Retweet>), typeof(Repository<Retweet>));
            services.AddScoped(typeof(Repository<Tweet>), typeof(Repository<Tweet>));
            services.AddScoped(typeof(Repository<User>), typeof(Repository<User>));

            services.AddIdentity<IdentityUser, IdentityRole>()
                   .AddEntityFrameworkStores<IdDbContext>();

            services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<IdDbContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                options.SlidingExpiration = true;

            });

            services.AddControllersWithViews();
            services.AddSession();
            services.AddDistributedMemoryCache();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(" / Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });



            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            */

        }
    }
}
