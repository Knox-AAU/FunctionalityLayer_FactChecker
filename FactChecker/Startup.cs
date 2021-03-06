using FactChecker.PassageRetrieval;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace FactChecker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        async void AppendRelationsToFile ()
        {
            TestData.WikiDataEntities wikiDataEntities = new TestData.WikiDataEntities();
            APIs.KnowledgeGraphAPI.KnowledgeGraphHandler handler = new APIs.KnowledgeGraphAPI.KnowledgeGraphHandler();
            IO.FileStreamHandler fileStreamHandler = new IO.FileStreamHandler();
            Console.WriteLine("begin");
            foreach(String s in wikiDataEntities.entities)
            {
                List<APIs.KnowledgeGraphAPI.KnowledgeGraphItem> triples = await handler.GetTriplesBySparQL(s, 2);
                foreach(APIs.KnowledgeGraphAPI.KnowledgeGraphItem triple in triples)
                {
                    fileStreamHandler.AppendToFile("./TestData/relations.txt", triple.ToString());
                }
            }
            Console.WriteLine("done");
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FactChecker", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FactChecker v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
