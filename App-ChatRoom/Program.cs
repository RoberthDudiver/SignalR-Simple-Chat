using App_ChatRoom.Data;
using App_ChatRoom.Process;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using App_ChatRoom.Validator;
using App_ChatRoom.Services;

namespace App_ChatRoom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IEndpointsProcess, EndpointsProcess>();
            builder.Services.AddScoped<ITokenValidator, TokenValidator>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddControllers();

            var app = builder.Build();
       
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapHub<ChatHub>("/chathub");
                       endpoints.MapControllers();

                   });
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}