using System;
using System.IO;
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
using System.Reflection;
using AutoMapper;
using GsCore.Api.Services;
using GsCore.Api.Services.Repository;
using GsCore.Api.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

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

            // generating ETags for HTTP caching 
            services.AddHttpCacheHeaders();

            // adding caching store with ResponseCaching Middleware
            services.AddResponseCaching();

            // register propertyMappingService
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            
            // register PropertyCheckerService
            services.AddTransient<IPropertyCheckerService,PropertyCheckerService>();

            // register AutoMapper
           // services.AddAutoMapper(typeof(Startup)); // OR
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.CacheProfiles.Add("240SecondsCacheProfile",
                    new CacheProfile()
                    {
                        Duration = 240
                    });
            })
            // START - Added NewtonsoftJson for PATCH request
            .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver=new CamelCasePropertyNamesContractResolver();
            })
            // END - Added NewtonsoftJson for PATCH request
            .AddXmlDataContractSerializerFormatters()
            
            .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type="https://gsapi.com/moelvalidationproblem",
                            Title = "One or more model validation errors occurred",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail="See the errors property for details.",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions.Add("TrackId",context.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = {"application/problem+json"}
                        };
                    };
                });


            ////services.Configure<MvcOptions>(config =>
            ////{
            ////    var newtonsoftJsonOutputFormatter =
            ////        config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

            ////    if (newtonsoftJsonOutputFormatter !=null)
            ////    {
            ////        newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.musicworld.hateoas+json");
            ////    }

            ////});


            #region "Add Api Versioning Service"
            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VV";
            });

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
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });


            var apiVersionDescriptionProvider =
                services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();


            #endregion


            #region "Swagger Service"

            services.AddSwaggerGen(setupAction =>
             {
                 foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                 {

                     setupAction.SwaggerDoc(
                         $"GsCoreApiSpecification{description.GroupName}",
                         new OpenApiInfo()
                         {
                             Title = "Gs Api",
                             Version = description.ApiVersion.ToString(),
                             Description = "Through Gs Api you can access artists and albums information",
                             Contact = new OpenApiContact()
                             {
                                 Email = "gsapi@gmail.com",
                                 Name = "Gs Api Group",
                                 Url =new Uri("https://www.gsapi.com")
                             },
                             License = new OpenApiLicense()
                             {
                                 Name = "MIT License",
                                 Url =new Uri("https://opensource.org/licenses/MIT")
                             }
                         });
                 }
                 setupAction.DocInclusionPredicate((documentName, apiDescription) =>
                 {
                     var actionApiVersionModel = apiDescription.ActionDescriptor
                         .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                     if (actionApiVersionModel == null)
                     {
                         return true;
                     }

                     if (actionApiVersionModel.DeclaredApiVersions.Any())
                     {
                         return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                             $"GsCoreApiSpecificationv{v.ToString()}" == documentName);
                     }

                     return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                         $"GsCoreApiSpecificationv{v.ToString()}" == documentName);


                 });



                 var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                 var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                 setupAction.IncludeXmlComments(xmlCommentsFullPath);
             });

            #endregion

            ////services.AddDbContext<GsDbContext> means GsDbContext is register with Scoped life time.
            services.AddDbContext<GsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            
            //// Here all the repository services uses the GsDbContext and GsDBContext is DbContext.
            //// that means we must register these services with scope that is equal to or shorter than DbContext (Scoped life time)
            //// we can not register with Singleton life time which is larger than DbContext life time.
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
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

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseHttpsRedirection();
            
            #region "Swagger & SwaggerUI middleware"

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerEndpoint($"/swagger/" +
                                                $"GsCoreApiSpecification{description.GroupName}/swagger.json",
                                         description.GroupName.ToUpper());
                }

                setupAction.RoutePrefix = "";
            });

            #endregion

            app.UseRouting();

           // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
