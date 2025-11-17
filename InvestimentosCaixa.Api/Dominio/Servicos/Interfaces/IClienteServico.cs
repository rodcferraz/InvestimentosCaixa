using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IClienteServico
    {
        Task<int> CadastrarClienteAsync(ClienteDTOCadastroRequest clienteDto);
        Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto);
        Task<bool> RemoverClienteAsync(int id);
        Task<ClienteDTOResponse> DetalhesClienteAsync(int id);
        Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync();
        Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string nomeCliente);
        Task<bool> AtualizarSenhaClienteAsync(int idCliente, string senhaAtual, string novaSenha);
    }
}
