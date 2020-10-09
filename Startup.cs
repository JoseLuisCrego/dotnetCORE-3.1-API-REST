using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commander.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;


namespace Commander
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
            //Añado a los controladores NewtonsoftJson para el tratamiento de datos json.
            services.AddControllers().AddNewtonsoftJson();

            /*
                Crea una instancia de un MockCommanderRepository por cada request del cliente (AddScoped) siempre 
                que alguna clase necesite de algo que implemente la interfaz ICommanderRepository vamos a 
                tener una instancia automáticamente de una clase MockCommanderRespository.
            */
            //services.AddScoped<ICommanderRepository, MockCommanderRepository>();

            //Hacemos lo propio para el repositorio real, y si en un futuro queremos probar el Mock, cambiamos uno por otro.
            services.AddScoped<ICommanderRepository, SqlCommanderRepository>();


            /*
                Añadimos aquí los contextos de BD necesarios para las conexiones que deseemos.
                Usaremos Configuration.GetConnectionString() para acceder a la propiedad que le especifiquemos 
                dentro de la propiedad json ConnectionStrings en AppSettings.json.
            */
            services.AddDbContext<CommanderContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CommanderConnection")));

            //Añado AutoMapper en todo el dominio para poder usar DI con éste donde sea que lo necesite.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
