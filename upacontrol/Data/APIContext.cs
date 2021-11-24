using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using upacontrol.Especialidade.Models;
using upacontrol.Login.Models;
using upacontrol.Medico.Models;
using static upacontrol.Login.Models.AuthModels;
using upacontrol.Agenda.Models;

namespace upacontrol.Data
{
    public class APIContext : IdentityDbContext<ApplicationUser>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        #region DB Especialidade
        public DbSet<EspecialidadeModel> EspecialidadeModel { get; set; }

        #endregion

        #region DB Medico
        public DbSet<MedicoModel> MedicoModel { get; set; }
        #endregion

        #region DB Auth

        public DbSet<Usuario> Usuario { get; set; }

        #endregion

        #region Agenda

        public DbSet<AgendamentoModel> Agenda { get; set; }  
        public DbSet<vwLocAgendamento> vwLocAgendamentos { get; set; }  

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
