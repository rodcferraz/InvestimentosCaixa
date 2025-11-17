using InvestimentosCaixa.Api.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Data.Context
{
    public class InvestimentosCaixaDbContext : DbContext
    {
        public InvestimentosCaixaDbContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Simulacao> Simulacoes => Set<Simulacao>();
        public DbSet<Investimento> Investimentos => Set<Investimento>();
        public DbSet<Telemetria> Telemetrias => Set<Telemetria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Investimento
            modelBuilder.Entity<Investimento>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Valor)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Relação com Cliente
                entity.HasOne(i => i.Cliente)
                    .WithMany(x => x.Investimentos)
                    .HasForeignKey(i => i.IdCliente);

                // Relação com Produto
                entity.HasOne(i => i.Produto)
                    .WithMany(x => x.Investimentos)
                    .HasForeignKey(i => i.IdProduto);
            });

            //Tabela juncao Investimento_Cliente
            modelBuilder.Entity<Simulacao>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.ValorInvestido)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(s => s.DataSimulacao)
                    .IsRequired();

                // Relação com Cliente
                entity.HasOne(s => s.Cliente)
                    .WithMany(x => x.Simulacoes)
                    .HasForeignKey(s => s.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relação com Produto
                entity.HasOne(s => s.Produto)
                    .WithMany(x => x.Simulacoes)
                    .HasForeignKey(s => s.IdProduto)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
