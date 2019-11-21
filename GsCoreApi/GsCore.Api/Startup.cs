using System;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using AutoMapper;
using GsCore.Api.Services.Repository;
using GsCore.Api.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace GsCore.Api
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

            //var gid = Guid.NewGuid();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;

            }).AddXmlDataContractSerializerFormatters();




            #region "Add Api Versioning Service"

            //services.AddVersionedApiExplorer(setupAction =>
            //{
            //    setupAction.GroupNameFormat = "'v'VV";
            //});

            /* Add api Versioning
              *
              *     1. The ReportApiVersions flag is used to add the API versions in the response header
              *     2. The AssumeDefaultVersionWhenUnspecified flag is used to set the default version
              *         when the client has not specified any versions. With this flag, the UnsupportedApiVersion
              *         exception will occur when the version is not specified by the client.
              *     3. The DefaultApiVersion flag is used to set the default version count.
              */

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                ////// options.ApiVersionReader = new HeaderApiVersionReader("x-version");
                ////options.ApiVersionReader = ApiVersionReader.Combine(
                ////    new HeaderApiVersionReader("x-version"),
                ////    new QueryStringApiVersionReader("v")
                ////    );
                options.DefaultApiVersion = new ApiVersion(1, 0);
                ////options.UseApiBehavior = false;
            });


            //var apiVersionDescriptionProvider =
            //    services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            #endregion




            ////services.AddDbContext<GsDbContext> means GsDbContext is register with Scoped life time.
            services.AddDbContext<GsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            
            //// Here all the repository services uses the GsDbContext and GsDBContext is DbContext.
            //// that means we must register these services with scope that is equal to or shorter than DbContext (Scoped life time)
            //// we can not register with Singleton life time which is larger than DbContext life time.
            //services.AddScoped<IGenresRepository, GenresRepository>();
            //services.AddScoped<IArtistRepository, ArtistsRepository>();
            //services.AddScoped<IAlbumRepository, AlbumRepository>();

            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        //TODO : Log this fault exception so that support team can see what exactly happened.
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault occured. Please try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

           // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
