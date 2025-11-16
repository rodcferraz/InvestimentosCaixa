namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias
{
    public class TelemetriaDTOResponse
    {
        public List<ServicoTelemetriaDTOResponse> Servicos { get; set; }
        public PeriodoTelemetriaDTOResponse Periodo { get; set; }
    }
}
