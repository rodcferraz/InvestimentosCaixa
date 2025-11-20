using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Realiza operações relacionadas a investimentos, como cadastro e listagem de investimentos por cliente.
    /// </summary>
    public interface IInvestimentoServico
    {
        // <summary>
        /// Cadastra o investimento solicitado pelo cliente
        /// </summary>
        /// <param name="investimentoDto">Dados de requisição para investimento</param>
        /// <returns>Retorna dados de investimento cadastrado</returns>
        Task<InvestimentoDTOResponse> CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest investimentoDto);

        /// <summary>
        /// Lista todos os investimentos realizados por um cliente específico
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        /// <returns>Lista de todos os investimentos do cliente</returns>
        Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int idCliente);

    }
}
