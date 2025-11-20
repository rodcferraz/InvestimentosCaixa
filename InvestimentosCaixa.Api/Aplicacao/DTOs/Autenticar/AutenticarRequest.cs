using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Autenticar
{
    /// <summary>
    /// Requisição para autenticação de cliente
    /// </summary>
    public class AutenticarRequest
    {
        /// <summary>
        /// Email do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }

        /// <summary>
        /// Senha do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Senha)} não informado.")]
        public string Senha { get; set; }
    }
}
