using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class ClienteDTOCadastroRequest : ClienteDTOBaseRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Senha)} não informado")]
        public string Senha { get; set; }
    }
}
