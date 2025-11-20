namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis
{
    /// <summary>
    /// Resposta para criação de perfil de cliente
    /// </summary>
    public class PerfilClienteDTOResponse
    {
        /// <summary>
        /// Id do cliente associado ao perfil
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Perfil risco do cliente gerado
        /// </summary>
        public string Perfil { get; set; }

        /// <summary>
        /// Pontuação do perfil de risco do cliente
        /// </summary>
        public decimal Pontuacao { get; set; }

        /// <summary>
        /// Descricão do perfil de risco do cliente
        /// </summary>
        public string Descricao { get; set; }
    }
}
