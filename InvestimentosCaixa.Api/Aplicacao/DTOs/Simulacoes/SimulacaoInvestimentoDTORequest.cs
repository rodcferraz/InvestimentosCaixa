using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    public class SimulacaoInvestimentoDTORequest
    {
        [Required(ErrorMessage = $"Campo {nameof(ClienteId)} não informado")]
        public int ClienteId { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(Valor)} não informado")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(PrazoMeses)} não informado")]
        public int PrazoMeses { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(TipoProduto)} não informado")]
        public string TipoProduto { get; set; }
    }
}
