using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    /// <summary>
    /// Dados cadastrais do cliente para resposta.
    /// </summary>
    public class ClienteDTOResponse
    {
        /// <summary>
        /// Id do cliente
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Email do cliente
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Enum: <see cref="PerfilRiscoClienteEnum"/>
        /// Valores: 1 - Conservador, 2 - Moderado, 3 - Agressivo
        /// Perfil de risco no cliente 
        /// </summary>
        public int Liquidez { get; set; }
    }
}
