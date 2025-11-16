namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias
{
    public class ServicoTelemetriaDTOResponse
    {
        public string Nome { get; set; }
        public int QuantidadeChamadas { get; set; }
        public int MediaTempoRespostaMs { get; set; }
    }
}
