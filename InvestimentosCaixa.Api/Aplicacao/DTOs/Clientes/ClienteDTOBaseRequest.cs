using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Apresentacao.Filtros;
using InvestimentosCaixa.Api.Dominio.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    /// <summary>
    /// Solicitação base para um cliente.
    /// </summary>
    public class ClienteDTOBaseRequest
    {
        /// <summary>
        /// Nome do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Nome)} não informado.")]
        public string Nome { get; set; }

        /// <summary>
        /// Email do cliente
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Email)} não informado.")]
        public string Email { get; set; }

        /// <summary>
        /// Perfil de risco no cliente 
        /// Valores: 1 - Conservador, 2 - Moderado, 3 - Agressivo
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Liquidez)} não informado.")]
        public int Liquidez { get; set; }
    }
}
