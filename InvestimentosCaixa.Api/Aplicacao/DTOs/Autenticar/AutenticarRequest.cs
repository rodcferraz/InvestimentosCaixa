using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Autenticar
{
    public class AutenticarRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(Senha)} não informado.")]
        public string Senha { get; set; }
    }
}
