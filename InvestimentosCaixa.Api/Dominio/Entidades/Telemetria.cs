using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Telemetria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NomeRota { get; set; }
        [Required]
        public long TempoResposta { get; set; }
        [Required]
        public DateTime DataRegistro { get; set; }
    }
}
