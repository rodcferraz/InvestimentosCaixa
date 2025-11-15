using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class ClienteDTORequest : ClienteDTOBaseRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Id)} não informado.")]
        public int Id { get; set; }
    }
}
