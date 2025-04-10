using FCamara.Test.Estacionamento.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCamara.Test.Estacionamento.Api.Data
{
    public class EstacionamentoDbContext : DbContext
    {
        public EstacionamentoDbContext(DbContextOptions<EstacionamentoDbContext> options) : base(options)
        {
        }

        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<RegistroEstacionamento> RegistrosEstacionamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Estabelecimento>()
                .HasIndex(e => e.CNPJ)
                .IsUnique();

            modelBuilder.Entity<Veiculo>()
                .HasIndex(v => v.Placa)
                .IsUnique();

            modelBuilder.Entity<RegistroEstacionamento>()
                .HasOne(r => r.Estabelecimento)
                .WithMany()
                .HasForeignKey(r => r.EstabelecimentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RegistroEstacionamento>()
                .HasOne(r => r.Veiculo)
                .WithMany()
                .HasForeignKey(r => r.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
