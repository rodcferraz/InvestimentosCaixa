using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos
{
    public class InvestimentoDTOBaseRequest
    {
        [Required]
        public string Tipo { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public decimal Rentabilidade { get; set; }
    }
}

