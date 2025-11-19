using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico
{
    public class GerarPerfilClienteServicoErroFake : IGerarPerfilClienteServico
    {
        public Task<PerfilClienteDTOResponse> GerarPerfilCiente(int idCliente)
        {
            throw new Exception("Error");
        }
    }
}
