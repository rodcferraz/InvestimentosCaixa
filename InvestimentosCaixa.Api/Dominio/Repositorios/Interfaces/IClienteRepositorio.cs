using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    public interface IClienteRepositorio : IGenericoRepositorio<Cliente>
    {
        Task<Cliente?> ListarClienteAtivoPorEmailAsync(string email);
        Task AtualizarSenhaClienteAsync(string email, string novaSenha);
    }
}
