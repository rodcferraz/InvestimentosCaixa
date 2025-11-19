using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Api.Infraestrutura.Repositorios;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.AutenticarControllerTestes.Repositorio
{
    public class ClienteRepositorioErroFake : GenericoRepositorio<Cliente>, IClienteRepositorio
    {
        private readonly InvestimentosCaixaDbContext _context;

        public ClienteRepositorioErroFake(InvestimentosCaixaDbContext context) :
            base(context)
        {
            _context = context;
        }

        public async Task AtualizarSenhaClienteAsync(string email, string novaSenha)
        {
            
        }

        public async Task<Cliente?> ListarClienteAtivoPorEmailAsync(string email)
        {
            throw new Exception("Error");
        }
    }
}
