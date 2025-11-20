using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    /// <summary>
    /// Classe utilizada para o cadastro de um cliente.
    /// </summary>
    public class ClienteDTOCadastroRequest : ClienteDTOBaseRequest
    {
        /// <summary>
        /// Senha de cadastro do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Senha)} não informado")]
        public string Senha { get; set; }
    }
}
