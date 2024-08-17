using MudBlazor.Services;
using Notebook.Blazor.Components;

namespace Notebook.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient("NotebookApi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("WebApiURL"));
            });

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();
<<<<<<< HEAD
=======

            builder.Services.AddHttpClient("WebApiURL", client => 
            client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("WebApiURL")));
>>>>>>> faff5ce7e8027fa5f241d45474814d0da9fce27c

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
                app.MapBlazorHub();
                app.MapFallbackToPage("/_Host");
            });
            
            app.Run();
        }
    }
}
