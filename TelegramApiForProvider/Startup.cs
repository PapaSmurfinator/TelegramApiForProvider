using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramApiForProvider.DbService;
using TelegramApiForProvider.Service;

namespace TelegramApiForProvider
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(connection));
            services.AddScheduler();
            services.AddTransient<ReminderService>();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<ISendService, SendService>();

            services.AddSingleton<ITelegramBotService>(provider =>
            {
                return new TelegramBotService(Configuration["Token"], Configuration["Url"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.ApplicationServices.UseScheduler(scheduler => scheduler.Schedule<ReminderService>().EveryMinute());

            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapControllers();
            });
        }
    }
}
