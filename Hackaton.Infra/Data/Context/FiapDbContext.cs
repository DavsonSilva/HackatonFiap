using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql;
using System.Data;
using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.NotificacaoEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Entities.UsuarioEntity;

namespace Hackaton.Infra.Data.Context
{
    public class FiapDbContext : DbContext
    {
        private readonly IDbConnection _connection;

        public FiapDbContext(DbContextOptions<FiapDbContext> options)
            : base(options)
        {
            _connection = new NpgsqlConnection(options.GetExtension<NpgsqlOptionsExtension>().ConnectionString);
        }

        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<Consulta> Consulta { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }
        public DbSet<Paciente> Pasciente { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    public class PostgreDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
