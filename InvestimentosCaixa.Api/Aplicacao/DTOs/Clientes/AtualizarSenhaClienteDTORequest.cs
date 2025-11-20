using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    /// <summary>
    /// Atualização de senha de cliente
    /// </summary>
    public class AtualizarSenhaClienteDTORequest
    {
        /// <summary>
        /// Email de cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }

        /// <summary>
        /// Senha atual do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(SenhaAtual)} atual não informado.")]
        public string SenhaAtual { get; set; }

        /// <summary>
        /// Nova senha do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(NovaSenha)} não informado.")]
        public string NovaSenha { get; set; }

        /// <summary>
        /// Confirmação de nova senha do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(ConfirmarNovaSenha)} não informado.")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
