using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class ClienteDTOBaseRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Nome)} não informado.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }
        [Required(ErrorMessage = $"Campo {nameof(Liquidez)} não informado.")]
        public int Liquidez { get; set; }
    }
}
