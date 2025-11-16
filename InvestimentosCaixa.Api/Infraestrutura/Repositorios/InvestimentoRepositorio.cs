using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    public class InvestimentoRepositorio : GenericoRepositorio<Investimento>, IInvestimentoRepositorio
    {
        private readonly InvestimentosCaixaDbContext _context;

        public InvestimentoRepositorio(InvestimentosCaixaDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Investimento>> ListarInvestimentosPorClienteAsync(int idCliente)
        {
            return await _context.Investimentos
                    .Include(i => i.InvestimentosCliente)
                        .ThenInclude(ic => ic.Produto)
                    .Where(i => i.InvestimentosCliente
                        .Any(ic => ic.ClienteId == idCliente))
                    .ToListAsync();
        }
    }
}
