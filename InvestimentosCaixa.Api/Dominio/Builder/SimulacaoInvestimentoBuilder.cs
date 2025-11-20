using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Builder
{
    /// <summary>
    /// Gerar resposta de simulação de investimento.
    /// </summary>
    public class SimulacaoInvestimentoBuilder
    {
        public ProdutoValidadoDTOResponse ProdutoValidado;
        public ResultadoSimulacaoDTOResponse ResultadoSimulacao;
        public DateTime DataSimulacao;
        public Produto Produto;
        public SimulacaoInvestimentoBuilder(Produto produto) 
        { 
            Produto = produto;
        }

        /// <summary>
        /// Adiciona informações do produto à simulação.
        /// </summary>
        public SimulacaoInvestimentoBuilder ComProdutoValidado()
        {
            ProdutoValidado = new ProdutoValidadoDTOResponse
            {
                Id = Produto.Id,
                Nome = Produto.Nome,
                Tipo = ((TipoProdutoEnum)Produto.Tipo).ToString(),
                Rentabilidade = Produto.Rentabilidade,
                Risco = ((RiscoProdutoEnum)Produto.Risco).ToString(),
            };
            return this;
        }

        /// <summary>
        /// Adiciona informações de cálculo de investimento à simulação
        /// </summary>
        /// <param name="simulacaoRequest"></param>
        /// <returns></returns>
        public SimulacaoInvestimentoBuilder ComResultadoSimulacao(SimulacaoInvestimentoDTORequest simulacaoRequest)
        {
            var rentabilidadeTotal = Produto.Rentabilidade /12 * simulacaoRequest.PrazoMeses;

            ResultadoSimulacao = new ResultadoSimulacaoDTOResponse
            {
                ValorFinal = simulacaoRequest.Valor * (1 + rentabilidadeTotal),
                RentabilidadeEfetiva = rentabilidadeTotal,
                PrazoMeses = simulacaoRequest.PrazoMeses
            };

            return this;
        }

        /// <summary>
        /// Adicioa data de simulação à resposta
        /// </summary>
        /// <returns></returns>
        public SimulacaoInvestimentoBuilder ComDataSimulacao(Simulacao simulacao)
        {
            DataSimulacao = simulacao.DataSimulacao;
            return this;
        }

        /// <summary>
        /// Agrega todas as informações e constrói o objeto de resposta
        /// </summary>
        /// <returns></returns>
        public SimulacaoInvestimentoDTOResponse Build()
        {
            return new SimulacaoInvestimentoDTOResponse
            {
                ProdutoValidado = ProdutoValidado,
                ResultadoSimulacao = ResultadoSimulacao,
                DataSimulacao = DataSimulacao
            };
        }

    }
}
