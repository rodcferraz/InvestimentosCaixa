using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Mapper
{
    public class SimulacaoMapperFake : ISimulacaoMapper
    {
        public List<SimulacaoProdutoDiaDTOResponse> ToDtoProdutoDiaList(List<Simulacao> simulacoes)
        {
            throw new NotImplementedException();
        }

        public SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao)
        {
            throw new NotImplementedException();
        }

        public List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes)
        {
            throw new NotImplementedException();
        }
    }
}
