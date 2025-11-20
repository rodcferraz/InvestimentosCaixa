using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Serviço responsável por gerenciar simulações de investimento.
    /// </summary>
    public interface ISimulacaoServico
    {
        /// <summary>
        /// Listar todas as simulações realizadas pelos clientes
        /// </summary>
        /// <returns>Lista todas as simulações realizadas pelos clientes</returns>
        Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos();

        /// <summary>
        /// Listar simulações de produtos efetuados no dia
        /// </summary>
        /// <returns>Lista com simulações efetuadas no dia</returns>
        Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia();

        // <summary>
        /// Realiza a simulação de investimento para um produto e cliente específico
        /// </summary>
        /// <param name="produto">Classe de produto utilizada para cadastro de investimento</param>
        /// <param name="simulacaoInvestimento">Informações para cadastro de simulações como Id do cliente, prazo, valor e rentabilidade</param>
        /// <returns>Simulação efetuada</returns>
        Task<SimulacaoInvestimentoDTOResponse?> SimularInvestimento(
            Produto produto, 
            SimulacaoInvestimentoDTORequest simulacaoInvestimento);
    }
}
