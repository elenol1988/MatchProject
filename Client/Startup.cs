using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Client.Client.Interfaces;
using JWTAuthentication;
using ClientProvider;
using Client.Client;
using NToastNotify;
using System.Text;

namespace Client
{
    public class Startup
    {
        public static string ApiUrl { get; private set; }

        public static string Audience { get; private set; }
        public static string Issuer { get; private set; }
        public static string Subject { get; private set; }

        public static string Key { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)

                    .AddNToastNotifyToastr(new ToastrOptions()
                    {
                        ProgressBar = true,
                        PositionClass = ToastPositions.TopCenter
                    });
            
            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
            services.AddScoped<IAccountClient, AccountClient>();
            services.AddScoped<IMatchClient, MatchClient>();
            services.AddScoped<IClientProvider, ClientProvider.ClientProvider>();
            services.AddScoped<IJWTokenManager, JWTokenManager>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],

                };

            })
            .AddCookie(
                o =>
                {
                    o.Cookie.Name = "match";
                    o.Cookie.IsEssential = true; // to bypass consent check
                    o.Cookie.HttpOnly = true; // Only send cookie through HTTP
                    o.Cookie.SameSite = SameSiteMode.None;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.LoginPath = new PathString("/Account/Login/");
                    o.AccessDeniedPath = new PathString("/Account/Login/");
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ApiUrl = Configuration["ApiUrl:Url"];
            Audience = Configuration["Jwt:Audience"];
            Key = Configuration["Jwt:Key"];
            Issuer = Configuration["Jwt:Issuer"];
            Subject = Configuration["Jwt:Subject"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
     
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
