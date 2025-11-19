using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico
{
    internal class ClienteServicoErroFake : IClienteServico
    {
        public Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto)
            => throw new Exception("Erro simulado");

        public Task<bool> AtualizarSenhaClienteAsync(string email, string senhaAtual, string novaSenha)
            => throw new Exception("Erro simulado");

        public Task<int> CadastrarClienteAsync(ClienteDTOCadastroRequest clienteDto)
            => throw new Exception("Erro simulado");

        public Task<ClienteDTOResponse> DetalhesClienteAsync(int id)
            => throw new Exception("Erro simulado");

        public Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string nomeCliente)
            => throw new Exception("Erro simulado");

        public Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync()
            => throw new Exception("Erro simulado");

        public Task<bool> RemoverClienteAsync(int id)
            => throw new Exception("Erro simulado");
    }
}
