using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using TSJ_CRI.Data;
using TSJ_CRI.Authentication;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Havit.Blazor.Components.Web;

namespace TSJ_CRI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHxServices();
            services.AddHxMessenger();
            services.AddSyncfusionBlazor();

            services.AddSingleton<UserAccountService>();
            services.AddSingleton<UserManageService>();

            services.AddScoped<ProtectedSessionStorage>();
            services.AddScoped<CustomAuth>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuth>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjY5NzE2QDMyMzAyZTMyMmUzME1LTUhpa21tejZEV0dsRGVrM1VDSkEvUjV0Q0UwQ2p6bVdvZlFPRWhYK009");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
