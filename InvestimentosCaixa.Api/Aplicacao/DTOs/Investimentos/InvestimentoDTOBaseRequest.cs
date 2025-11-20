using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos
{
    /// <summary>
    /// Requisição base para realizar um investimento
    /// </summary>
    public class InvestimentoDTOBaseRequest
    {
        /// <summary>
        /// Id de cliente que está realizando o investimento
        /// </summary>
        [Required]
        public int IdCliente { get; set; }

        /// <summary>
        /// Id do produto no qual o investimento será realizado
        /// </summary>
        [Required]
        public int IdProduto { get; set; }

        /// <summary>
        /// Valor a ser aplicado no investimento
        /// </summary>
        [Required]
        public decimal Valor { get; set; }
    }
}

