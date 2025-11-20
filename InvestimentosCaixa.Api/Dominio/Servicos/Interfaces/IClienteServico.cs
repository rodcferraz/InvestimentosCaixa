using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Classe de serviço responsável pelas operações relacionadas aos clientes.
    /// </summary>
    public interface IClienteServico
    {
        /// <summary>
        /// Realiza o cadastro de um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Dados de requisição para a crição de um novo cliente</param>
        /// <returns>Retorna Id do cliente</returns>
        Task<int> CadastrarClienteAsync(ClienteDTOCadastroRequest clienteDto);

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="clienteDto">Dto do cliente</param>
        /// <returns>Cliente atualizado</returns>
        /// <exception cref="ConvertEnumException">Lança exceção caso o <see cref="PerfilRiscoClienteEnum"/>
        /// não esteja definido internamente</exception>
        Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto);


        /// <summary>
        /// Remove logicamente um cliente pelo Id.
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Retorna confirmação da deleção lógica do cliente</returns>
        Task<bool> RemoverClienteAsync(int id);

        /// <summary>
        /// Detalhar um cliente pelo Id.
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Dados do cliente</returns>
        Task<ClienteDTOResponse> DetalhesClienteAsync(int id);

        /// <summary>
        /// Listar todos os clientes ativos
        /// </summary>
        /// <returns>Lista de todos os clientes ativos</returns>
        Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync();

        /// <summary>
        /// Lista clientes ativos por email
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <returns>Retorna dados do cliente</returns>
        Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string nomeCliente);

        /// <summary>
        /// Atualiza a senha do cliente
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <param name="senhaAtual">Senha atual do cliente</param>
        /// <param name="novaSenha">Nova senha do cliente</param>
        /// <returns>Confirmação de atualização de senha</returns>
        /// <exception cref="SenhaIncorretaException"></exception>
        Task<bool> AtualizarSenhaClienteAsync(string email, string senhaAtual, string novaSenha);
    }
}
