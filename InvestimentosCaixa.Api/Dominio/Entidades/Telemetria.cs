namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Telemetria
    {
        public int Id { get; set; }
        public string NomeRota { get; set; }
        public long TempoResposta { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}
