using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class ClienteDTOBaseRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Nome)} não informado.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(RendaMensal)} não informado.")]
        public decimal RendaMensal { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(PercentualInvestimentoRenda)} não informado.")]
        public decimal PercentualInvestimentoRenda { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(PerfilDeclarado)} não informado.")]
        public int PerfilDeclarado { get; set; }
    }
}
