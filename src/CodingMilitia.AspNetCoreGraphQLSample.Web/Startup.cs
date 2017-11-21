using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodingMilitia.AspNetCoreGraphQLSample.Data;
using CodingMilitia.AspNetCoreGraphQLSample.Web.Middleware;
using CodingMilitia.AspNetCoreGraphQLSample.Web.Schema;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web
{
    public class Startup
    {
        private static readonly string ConnectionString = "server=localhost;port=5432;user id=user;password=pass;database=AspNetCoreGraphQLSample";
        private ServiceProvider _serviceProvider;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SampleSchema>();
            services.AddScoped<ObjectGraphType, EFBasedSampleQuery>();
            services.AddDbContext<SampleContext>(options => options.UseNpgsql(ConnectionString));

            _serviceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMiddleware<GraphQLMiddleware>();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("No middleware handled the request");
            });

            using (var db = _serviceProvider.GetRequiredService<SampleContext>())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.EnsureSeedData();
            }

        }
    }
}
