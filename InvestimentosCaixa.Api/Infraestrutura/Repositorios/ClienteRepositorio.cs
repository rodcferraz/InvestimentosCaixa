using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Api.Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    public class ClienteRepositorio : GenericoRepositorio<Cliente>, IClienteRepositorio
    {
        private readonly InvestimentosCaixaDbContext _context;

        public ClienteRepositorio(InvestimentosCaixaDbContext context) : 
            base(context)
        {
            _context = context;
        }

        public async Task AtualizarSenhaClienteAsync(int idCliente, string novaSenha)
        {
            await _context.Clientes
                .Where(c => c.Id == idCliente)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.SenhaHash, novaSenha));

            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> ListarClienteAtivoPorEmailAsync(string email)
        {
            return await _context.Clientes
                        .FirstOrDefaultAsync(p => 
                            p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
