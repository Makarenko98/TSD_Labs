using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lab4.BLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lab4.BLL.Services;
using System.IO;
using System.Data.SqlClient;
using Dapper;

namespace Lab4.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            EnsureFileStorageCreated(Configuration.GetValue<string>("FileStoragePath"));
            var connectionString = Configuration.GetConnectionString("SocialNet");
            EnsureDatabaseCreated(connectionString);
            InitData(connectionString, Configuration.GetValue<string>("DataInitializersDirectoryPath"));
        }

        private void EnsureDatabaseCreated(string connectionString)
        {
            using (var db = new SocialNetDbContext(connectionString)) {
                db.Database.EnsureCreated();
            }
        }

        private void InitData(string connectionString, string dataInitializersDirectoryPath)
        {
            if (!Directory.Exists(dataInitializersDirectoryPath))
                return;
            string initSql = "";
            using (var db = new SqlConnection(connectionString)) {
                foreach (var path in Directory.GetFiles(dataInitializersDirectoryPath)) {
                    initSql = "";
                    using (var reader = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read))) {
                        initSql = reader.ReadToEnd();
                    }
                    db.Execute(initSql);
                }
            }
        }

        private void EnsureFileStorageCreated(string storagePath)
        {
            if (!Directory.Exists(storagePath)) {
                Directory.CreateDirectory(storagePath);
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("SocialNet");
            string fileStoragePath = Configuration.GetValue<string>("FileStoragePath");
            services.AddSingleton(new UserService(connectionString));
            services.AddSingleton(new FriendService(connectionString));
            services.AddSingleton(new ChatService(connectionString));
            services.AddSingleton(new UserPhotoService(connectionString, fileStoragePath));

            services.AddDbContext<SocialNetDbContext>(options => options.UseSqlServer(connectionString));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes => {
                routes.MapHub<ChatHub>("/ChatHub");
            });
            
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
