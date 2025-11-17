using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class AtualizarSenhaClienteDTORequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(SenhaAtual)} atual não informado.")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(NovaSenha)} não informado.")]
        public string NovaSenha { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(ConfirmarNovaSenha)} não informado.")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
