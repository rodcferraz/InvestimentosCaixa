using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    /// <summary>
    /// Classe voltada para a atualização de um cliente.
    /// </summary>
    public class ClienteDTORequest : ClienteDTOBaseRequest
    {
        /// <summary>
        /// Id do cliente a ser atualizado
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Id)} não informado.")]
        public int Id { get; set; }
    }
}
