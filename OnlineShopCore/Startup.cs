using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using OnlineShopCore.Application.Dapper.Implementation;
using OnlineShopCore.Application.Dapper.Intefaces;
using OnlineShopCore.Application.Implementation;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Authorization;
using OnlineShopCore.Data.EF;
using OnlineShopCore.Data.EF.Repositories;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Helpers;
using OnlineShopCore.Hubs;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Services;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineShopCore
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsAssembly("OnlineShopCore.Data.EF")));

            services.AddIdentity<AppUser, AppRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });
            services.AddAutoMapper();
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            }); ;
            services.AddAuthentication()
                .AddFacebook(facebookOpts =>
                {
                    facebookOpts.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOpts.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOpts =>
                {
                    googleOpts.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOpts.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });
            // Add application services.
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();

            services.AddTransient<DbInitializer>();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();
            //add pwa service
            services.AddProgressiveWebApp();

            services.AddMvc().AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(
             opts =>
             {
                 var supportedCultures = new List<CultureInfo>
                 {
                        new CultureInfo("en-US"),
                        new CultureInfo("vi-VN")
                 };

                 opts.DefaultRequestCulture = new RequestCulture("en-US");
                 // Formatting numbers, dates, etc.
                 opts.SupportedCultures = supportedCultures;
                 // UI strings that we have localized.
                 opts.SupportedUICultures = supportedCultures;
             });


            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            //Repositories
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IBillDetailRepository, BillDetailRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<IProductImageRepository, ProductImageRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IPublisherCompanyRepository, PublisherCompanyRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();

            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<ISlideImageRepository, SlideImageRepository>();

            //Services
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<IFeedbackService, FeedbackService>();

            //Report Service
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IUserReportService, UserReportService>();
            services.AddTransient<IOrderReportService, OrderReportService>();

            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IPublisherService, PublisherService>();

            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            //SignalR
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler("/Home/Error");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseSession();

            app.UseStaticFiles();

            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

        }
    }
}