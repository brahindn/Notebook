using Microsoft.AspNetCore.Mvc.RazorPages;
using MudBlazor.Services;

namespace Notebook.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddMudServices();

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetConnectionString("WebApiURL"))
            });

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddControllers();

            builder.Services.Configure<RazorPagesOptions>(options =>
            {
            options.RootDirectory = "/Pages";
            });

            var app = builder.Build();

             if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();  
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage(page: "/Home");
            });

            app.Run();
        }
    }
}
