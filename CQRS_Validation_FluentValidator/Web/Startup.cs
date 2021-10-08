using System;
using System.IO;
using Application.Behaviors;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using Web.Middleware;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

            services.AddControllers()
                .AddApplicationPart(presentationAssembly);

            services.AddSwaggerGen(c =>
            {
                string presentationDocumentationFile = $"{presentationAssembly.GetName().Name}.xml";

                string presentationDocumentationFilePath = Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);

                c.IncludeXmlComments(presentationDocumentationFilePath);

                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Web", Version = "v1"});
            });

            services.AddDbContextPool<ApplicationDbContext>(builder =>
            {
                var connectionString = Configuration.GetConnectionString("Database");

                builder.UseNpgsql(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ExceptionHandlingMiddleware>();

            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

            services.AddMediatR(applicationAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(applicationAssembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
