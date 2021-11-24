using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using upacontrol.Agenda.Repository;
using upacontrol.Data;
using upacontrol.Especialidade.Repository;
using upacontrol.Login.Repository;
using upacontrol.Medico.Repository;

namespace upacontrol
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            env = environment;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Service especialidade
            services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IAuthRepository, IAuthRepository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (env.IsDevelopment())
            {
                string SERVER = Configuration.GetConnectionString("SERVER");
                string DATABASE = Configuration.GetConnectionString("DATABASE");
                string USERDB = Configuration.GetConnectionString("USERDB");
                string PASSWORDUSER = Configuration.GetConnectionString("PASSWORDUSER");

                services.AddDbContext<APIContext>(options =>
                 options.UseSqlServer("Server=" + SERVER + ";Database=" + DATABASE + ";Trusted_Connection=false;user ID=" + USERDB + ";password=" + PASSWORDUSER + ";MultipleActiveResultSets=true"));
            }
            else if (env.IsProduction())
            {
                string SERVER = Environment.GetEnvironmentVariable("SERVER");
                string DATABASE = Environment.GetEnvironmentVariable("DATABASE");
                string USERDB = Environment.GetEnvironmentVariable("USERDB");
                string PASSWORDUSER = Environment.GetEnvironmentVariable("PASSWORDUSER");

                services.AddDbContext<APIContext>(options =>
                 options.UseSqlServer("Server=" + SERVER + ";Database=" + DATABASE + ";Trusted_Connection=false;user ID=" + USERDB + ";password=" + PASSWORDUSER + ";MultipleActiveResultSets=true"));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
