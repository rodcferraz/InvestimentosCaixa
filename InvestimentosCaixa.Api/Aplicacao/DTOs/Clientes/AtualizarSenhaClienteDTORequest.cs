using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class AtualizarSenhaClienteDTORequest
    {
        [Required(ErrorMessage = "Campo Id não informado.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Senha atual não informado.")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = "Campo Nova senha não informado.")]
        public string NovaSenha { get; set; }
        [Required(ErrorMessage = "Campo Confirmar nova senha não informado.")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
