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
    }
}
