using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Registration.Blueprint.Commands;
using HotelDomain = Registration.Application.Bootstrap;

namespace web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            HotelDomain.AsService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.Run( async (context) => {
                var page = context.Request.Path.ToUriComponent();
                if (page.EndsWith(".html") || page.Equals("/")) RouteToPage(page, context);
                else RouteToCommandHandler(page, context);
            });
        }

        private static void RouteToCommandHandler(string commandHandler, HttpContext context)
        {
            switch (commandHandler) {
                case "/add-room":

                    HotelDomain.MainBus.Publish(new AddRoom(Guid.NewGuid(),
                       context.Request.Form["roomNumber"].ToString(),
                       context.Request.Form["roomLocation"].ToString(),
                       context.Request.Form["roomType"].ToString()));
                    break;
                case "/list-rooms":
                    context.Response.WriteAsync(Json.Serialize(HotelDomain.RoomReadModel.Current.Select(r=>r.Summary).ToArray()));
                    break;
                default:
                    context.Response.SendFileAsync("pages/401.html");
                    break;
            }
        }

        private static void RouteToPage(string page, HttpContext context)
        {
            if (context.Request.Path == "/" || page.Equals("/")) { page = "index.html"; }

            try {
                context.Response.WriteAsync(File.ReadAllText($"pages/{page}"));
            }
            catch (Exception e) {
                context.Response.SendFileAsync("pages/401.html");
            }

        }
    }
}