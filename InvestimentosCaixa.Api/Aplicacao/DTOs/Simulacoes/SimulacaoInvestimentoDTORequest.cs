using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Requisição para simulação de investimento
    /// </summary>
    public class SimulacaoInvestimentoDTORequest
    {
        /// <summary>
        /// Id do cliente que irá realizar a simulação
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(ClienteId)} não informado")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Valor que será simulado
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Valor)} não informado")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Prazo em meses da simulação
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(PrazoMeses)} não informado")]
        public int PrazoMeses { get; set; }

        /// <summary>
        /// <summary>
        /// Tipo de produto
        /// Valores: 
        /// TesouroSelic,
        /// CDB,
        /// LCI,
        /// LCA,
        /// TesouroIPCA,
        /// Fundo,
        /// Acoes,
        /// ETFs,
        /// Criptomoeda
        /// </summary>
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(TipoProduto)} não informado")]
        public string TipoProduto { get; set; }
    }
}
