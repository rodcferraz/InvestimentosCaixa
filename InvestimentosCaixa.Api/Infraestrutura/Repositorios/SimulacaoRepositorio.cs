using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    public class SimulacaoRepositorio : GenericoRepositorio<Simulacao>, ISimulacaoRepositorio
    {
        private readonly InvestimentosCaixaDbContext _context;
        public SimulacaoRepositorio(InvestimentosCaixaDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public override async Task<List<Simulacao>> ListarTodosAsync()
        {
            return await _context.Simulacoes
                .Include(x => x.Produto)
                .ToListAsync();
        }
    }
}
