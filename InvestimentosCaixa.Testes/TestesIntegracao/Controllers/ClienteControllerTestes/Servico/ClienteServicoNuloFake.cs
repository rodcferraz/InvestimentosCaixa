using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico
{
    public class ClienteServicoNuloFake : IClienteServico
    {
        public Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto)
        {
            return Task.FromResult<ClienteDTOResponse>(null);
        }

        public Task<bool> AtualizarSenhaClienteAsync(string email, string senhaAtual, string novaSenha)
        {
            throw new NotImplementedException();
        }

        public Task<int> CadastrarClienteAsync(ClienteDTOCadastroRequest clienteDto)
        {
            throw new NotImplementedException();
        }

        public Task<ClienteDTOResponse> DetalhesClienteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string nomeCliente)
        {
            return Task.FromResult<ClienteDTOResponse>(null);
        }

        public Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync()
        {
            return Task.FromResult<List<ClienteDTOResponse>>(null);
        }

        public Task<bool> RemoverClienteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
