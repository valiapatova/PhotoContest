using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PhotoContest.Data;
using PhotoContest.Helpers;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Repositories.AuthRepository;
using PhotoContest.Repositories.CategoriesRepository;
using PhotoContest.Repositories.ContestsRepository;
using PhotoContest.Repositories.PhotoPostsRepository;
using PhotoContest.Repositories.RatingsRepository;
using PhotoContest.Repositories.UsersRepository;
using PhotoContest.Services.CategoriesService;
using PhotoContest.Services.ContestsService;
using PhotoContest.Services.PhotoPostsServices;
using PhotoContest.Services.RatingsService;
using PhotoContest.Services.UsersService;
using System;

namespace PhotoContest
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

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                      options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "PhotoContest.API", Version = "v2" });
            });

            //EF
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //Session
            services.AddSession(options =>
            {
                // With IdleTimeout you can change the number of seconds after which the session expires.
                // The seconds reset every time you access the session.
                // This only applies to the session, not the cookie.
                options.IdleTimeout = TimeSpan.FromSeconds(200);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //services.AddScoped<IConfiguration, Configuration>();

            //Repositories
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IContestsRepository, ContestsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IPhotoPostsRepository, PhotoPostsRepository>();
            services.AddScoped<IRatingsRepository, RatingsRepository>();

            //Services
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IContestsService, ContestsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IPhotoPostsService, PhotoPostsService>();
            services.AddScoped<IRatingsService, RatingsService>();

            //Helpers
            services.AddTransient<IAuthorizationHelper, AuthorizationHelper>();

            //Mappers
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IContestMapper, ContestMapper>();
            services.AddTransient<ICategoryMapper, CategoryMapper>();
            services.AddTransient<IPhotoPostMapper, PhotoPostMapper>();
            services.AddTransient<IRatingMapper, RatingMapper>();
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
                app.UseExceptionHandler("/Home/Error");
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "PhotoContest");

            });                        

            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
