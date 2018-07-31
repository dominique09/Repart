using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repart.Common.Auth;
using Repart.Common.Exceptions;
using Repart.Common.Mongo;
using Repart.Services.Intervenants.Domain.Repositories;
using Repart.Services.Intervenants.Repositories;
using Repart.Services.Intervenants.Services;

namespace Repart.Services.Intervenants
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
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            services.AddLogging();
            
            mvcBuilder.AddMvcOptions(o => { o.Filters.Add(new RepartExceptionFilter()); });
            
            services.AddJwt(Configuration);
            services.AddMongoDb(Configuration);
            services.AddScoped<IIntervenantRepository, IntervenantRepository>();
            services.AddScoped<IIntervenantService, IntervenantService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();
            app.UseMvc();
        }
    }
}
