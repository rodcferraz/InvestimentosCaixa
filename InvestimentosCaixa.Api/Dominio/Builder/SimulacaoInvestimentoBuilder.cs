using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Builder
{
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

        public SimulacaoInvestimentoBuilder ComProdutoValidado()
        {
            ProdutoValidado = new ProdutoValidadoDTOResponse
            {
                Id = Produto.Id,
                Nome = Produto.Nome,
                Tipo = ((TipoProdutoEnum)Produto.Tipo).ToString(),
                Rentabilidade = Produto.Rentabilidade,
                Risco = ((RiscoProduto)Produto.Risco).ToString(),
            };
            return this;
        }

        public SimulacaoInvestimentoBuilder ComResultadoSimulacao(SimulacaoInvestimentoDTORequest simulacaoRequest)
        {
            var rentabilidadeTotal = Produto.Rentabilidade /12 * simulacaoRequest.PrazoMeses;

            ResultadoSimulacao = new ResultadoSimulacaoDTOResponse
            {
                ValorFinal = simulacaoRequest.Valor * rentabilidadeTotal,
                RentabilidadeEfetiva = rentabilidadeTotal,
                PrazoMeses = simulacaoRequest.PrazoMeses
            };

            return this;
        }

        public SimulacaoInvestimentoBuilder ComDataSimulacao(Simulacao simulacao)
        {
            DataSimulacao = simulacao.DataSimulacao;
            return this;
        }

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
