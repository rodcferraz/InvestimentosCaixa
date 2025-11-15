using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ISimulacaoMapper
    {
        SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao);
        //Simulacao ToBaseEntity(SimulacaoDTOBaseRequest simulacaoDto);
        //Simulacao ToEntity(SimualacaoDTORequest clienteDto);
        List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes);
    }
}
