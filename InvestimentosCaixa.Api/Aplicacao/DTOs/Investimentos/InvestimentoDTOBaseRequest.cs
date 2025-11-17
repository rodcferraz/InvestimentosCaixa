using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos
{
    public class InvestimentoDTOBaseRequest
    {
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdProduto { get; set; }
        [Required]
        public decimal Valor { get; set; }
    }
}

