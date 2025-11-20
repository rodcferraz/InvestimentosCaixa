using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    /// <summary>
    /// Entidade que representa telemetria no sistema de investimentos
    /// </summary>
    public class Telemetria
    {
        /// <summary>
        /// Id de telemetria e será a chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da rota acessada
        /// </summary>
        [Required]
        public string NomeRota { get; set; }

        /// <summary>
        /// Tempo de resposta da requisição em milissegundos
        /// </summary>
        [Required]
        public long TempoResposta { get; set; }

        /// <summary>
        /// Data de registro da telemetria
        /// </summary>
        [Required]
        public DateTime DataRegistro { get; set; }
    }
}
